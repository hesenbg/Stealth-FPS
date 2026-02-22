using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject ragdollHips;
    [SerializeField] float RagdollDisableTimer = 5f;
    private float CurrRagdollDisableTimer =0;

    public void MatchRagdollToAnimation(GameObject originalHips)
    {
        CopyTransformRecursively(originalHips.transform, ragdollHips.transform);
    }
    private void CopyTransformRecursively(Transform source, Transform destination)
    {
        destination.position = source.position;
        destination.rotation = source.rotation;

        foreach (Transform sourceChild in source)
        {
            Transform destinationChild = destination.Find(sourceChild.name);
            if (destinationChild != null)
            {
                CopyTransformRecursively(sourceChild, destinationChild);
            }
        }
    }

    private bool hasCleaned = false; // Guard to run logic only once

    private void Update()
    {
        Debug.Log(CurrRagdollDisableTimer);
        if (CurrRagdollDisableTimer < RagdollDisableTimer )
        {
            CurrRagdollDisableTimer += Time.deltaTime;
        }
        else if (!hasCleaned)
        {
            CleanHip();
            hasCleaned = true;
        }
    }

    private void CleanHip()
    {
        Rigidbody[] rbs = ragdollHips.GetComponentsInChildren<Rigidbody>();
        Collider[] cols = ragdollHips.GetComponentsInChildren<Collider>();

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true; // Stops physics movement
        }
        foreach (Collider col in cols)
        {
            col.enabled = false; // Stops collision checks
        }

        Debug.Log("Physics stripped and Hips switched.");

        this.enabled = false;
    }

}