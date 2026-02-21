using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject ragdollContainer; // The parent of the ragdoll hierarchy
    [SerializeField] GameObject ragdollHips;
    [SerializeField] float RagdollDisableTimer = 5f;

    private GameObject CleanHips; 
    private GameObject CleanContainer; // The parent of the animated/clean hierarchy
    private float CurrRagdollDisableTimer;
    private bool isSyncing = false;

    public void MatchRagdollToAnimation(GameObject originalHips)
    {
        CleanHips = originalHips;
        CopyTransformRecursively(originalHips.transform, ragdollHips.transform);
    }
    public void StartRagdollSequence(GameObject originalHips, GameObject originalContainer)
    {
        CleanHips = originalHips;
        CleanContainer = originalContainer;

        // 1. Match Ragdoll to the current Animation pose before starting physics
        CopyTransformRecursively(CleanHips.transform, ragdollHips.transform);

        // 2. Enable Ragdoll, Disable Clean version
        ragdollContainer.SetActive(true);
        CleanContainer.SetActive(false);

        // 3. Start the timer
        CurrRagdollDisableTimer = RagdollDisableTimer;
        isSyncing = true;
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
            // TIMER FINISHED: Perform the swap
            SwapRagdollForClean();
        }
    }

    private void SwapRagdollForClean()
    {
        isSyncing = false;

        // 1. Final sync: Move the "Clean" bones to exactly where the Ragdoll landed
        CopyTransformRecursively(ragdollHips.transform, CleanHips.transform);

        // 2. Swap the objects
        CleanContainer.SetActive(true);
        ragdollContainer.SetActive(false); 

        Debug.Log("Physics Ragdoll swapped for optimized Clean bones.");
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
}