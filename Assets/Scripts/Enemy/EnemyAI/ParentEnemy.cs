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
        if (closestEnemy == null)
            return;

        if (closestEnemy != null)
           TriggerEnemy(position, minDistance);
           return;
    }

    void TriggerEnemy(Vector3 triggerPosition, float minDistance)
    {
        BaseAI closest = closestEnemy.GetComponent<BaseAI>();
        Debug.Log(closest.gameObject.name);
        if (closest == null)
            return; // safety check

        if (minDistance < closest.DetectionRange)
        {
            closest.IsEnemyDistracted = true;
            closest.TriggerPosition = triggerPosition;
            //closest.Agent.enabled = true;
        }
    }
}
