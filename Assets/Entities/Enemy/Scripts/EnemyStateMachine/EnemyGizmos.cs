using UnityEngine;
using System;
public class EnemyGizmos : MonoBehaviour
{
    [Serializable]
    struct EnemyEntry
    {
        public EnemyAIData data;
        public Transform root;
    }

    [SerializeField] EnemyEntry[] enemies;
    [SerializeField] float radius;

    Vector3[][] SpherePos;

    private void Start()
    {
        SpherePos = new Vector3[enemies.Length][];
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyAIData data = enemies[i].data;
            Transform root = enemies[i].root;
            if (data == null || root == null || data.PatrolPositions == null) continue;
            SpherePos[i] = new Vector3[data.PatrolPositions.Length];
            for (int j = 0; j < data.PatrolPositions.Length; j++)
            {
                SpherePos[i][j] = root.TransformPoint(data.PatrolPositions[j].Position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (enemies == null) return;
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyAIData data = enemies[i].data;
            Transform root = enemies[i].root;
            if (data == null || root == null || data.PatrolPositions == null) continue;
            for (int j = 0; j < data.PatrolPositions.Length; j++)
            {
                Vector3 worldPos = root.TransformPoint(data.PatrolPositions[j].Position);
                Vector3 pos = SpherePos == null ? worldPos : SpherePos[i][j];
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(pos, radius);

                int next = (j + 1) % data.PatrolPositions.Length;
                Vector3 nextWorldPos = root.TransformPoint(data.PatrolPositions[next].Position);
                Vector3 nextPos = SpherePos == null ? nextWorldPos : SpherePos[i][next];
                Gizmos.DrawLine(pos, nextPos);
            }
        }
    }
}