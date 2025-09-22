using UnityEngine;
using UnityEngine.UIElements;

public class ParentEnemy : MonoBehaviour
{
    public static ParentEnemy Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject); 
    }

    Transform closestEnemy;

    public void TriggerClosestEnemy(Vector3 position)
    {
        closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform enemy in transform)
        {
            float distance = Vector3.Distance(position, enemy.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        if( closestEnemy != null )
            TriggerEnemy(position, minDistance);
    }

    void TriggerEnemy(Vector3 TriggerPosition,float MinDis)
    {
        GuardingEnemy closest = closestEnemy.gameObject.GetComponent<GuardingEnemy>();
        if(MinDis < closest.DetectionRange)
        {
            closest.IsEnemyDistracted = true;
            closest.TriggerPosition = TriggerPosition;
            closest.EnemyNavMesh.enabled = true;
        }
    }
}
