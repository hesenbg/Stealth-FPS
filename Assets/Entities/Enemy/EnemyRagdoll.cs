using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField] GameObject ragdollHips;

    public void MatchRagdollToAnimation(GameObject originalHips)
    {
        CopyTransformRecursively(originalHips.transform, ragdollHips.transform);
    }

    private void CopyTransformRecursively(Transform source, Transform destination)
    {
        // Sync position and rotation
        destination.position = source.position;
        destination.rotation = source.rotation;

        // Iterate through all children and match them by name
        foreach (Transform sourceChild in source)
        {
            // Find the matching bone in the ragdoll hierarchy
            Transform destinationChild = destination.Find(sourceChild.name);

            if (destinationChild != null)
            {
                CopyTransformRecursively(sourceChild, destinationChild);
            }
        }
    }
}