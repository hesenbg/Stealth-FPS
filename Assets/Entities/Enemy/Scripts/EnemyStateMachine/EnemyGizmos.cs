using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    [SerializeField] EnemyAIData data;
    [SerializeField] float Radius;


    private void OnDrawGizmos()
    {
        for(int i= 0; i < data.PatrolPositions.Length; i++)
        {
            Gizmos.DrawWireSphere(data.PatrolPositions[i], Radius);
        }
    }
}
