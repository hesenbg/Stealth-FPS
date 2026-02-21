using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject ragdollContainer; // The parent of the ragdoll hierarchy
    [SerializeField] GameObject ragdollHips;
    [SerializeField] float RagdollDisableTimer = 5f;

    private GameObject CleanHipObject; 
    private GameObject CleanContainer; // The parent of the animated/clean hierarchy
    private float CurrRagdollDisableTimer;
    private bool isSyncing = false;

    public void MatchRagdollToAnimation(GameObject originalHips)
    {
        CleanHipObject = originalHips;
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

    private void CleanHip() // applies the transform values of a ragdol hip and siwtches them(to avoid calculating the ragdoll physics)
    {

    }

    private void Update()
    {
        if (!isSyncing) return;

        if (CurrRagdollDisableTimer > 0)
        {
            CurrRagdollDisableTimer -= Time.deltaTime;
        }
        else
        {

        }
    }

}