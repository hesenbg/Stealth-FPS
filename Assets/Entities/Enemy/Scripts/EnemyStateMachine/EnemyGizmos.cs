using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    [SerializeField] EnemyAIData[] data;
    [SerializeField] float radius;

    private void OnDrawGizmos()
    {
        if (data == null) return;

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == null || data[i].PatrolPositions == null) continue;

            for (int j = 0; j < data[i].PatrolPositions.Length; j++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(data[i].PatrolPositions[j], radius);

                if (j < data[i].PatrolPositions.Length - 1)
                {
                    Gizmos.DrawLine(data[i].PatrolPositions[j], data[i].PatrolPositions[j + 1]);
                }
            }
        }
    }
}