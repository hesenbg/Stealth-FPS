using UnityEngine;
using System.Collections;

public class BrokenGlass : MonoBehaviour
{
    Rigidbody[] rbs;
    Collider[] colliders;

    [SerializeField] float Force;
    [SerializeField] float cleanupDelay = 3f;

    Vector3 MainCenter;

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        MainCenter = transform.position;

        foreach (var rb in rbs)
        {
            Vector3 center = rb.gameObject.GetComponent<Collider>().bounds.center;
            Vector3 ExplosionForceDirection = (center - MainCenter).normalized;

            rb.AddForce(ExplosionForceDirection * Force, ForceMode.Impulse);
        }

        StartCoroutine(CleanupPhysics());
    }

    private IEnumerator CleanupPhysics()
    {
        yield return new WaitForSeconds(cleanupDelay);

        foreach (var coll in colliders)
        {
            if (coll != null)
            {
                Destroy(coll);
            }
        }

        foreach (var rb in rbs)
        {
            if (rb != null)
            {
                Destroy(rb);
            }
        }
    }
}