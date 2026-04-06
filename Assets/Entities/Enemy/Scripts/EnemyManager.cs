using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    GameObject[] enemies;
    HealthManager[] EnemyHealths;
    GameObject closestObject;

    private void Awake()
    {
        instance = this;
        EnemyHealths = GetComponentsInChildren<HealthManager>();
    }

    public void CheckEnemies()
    {
        List<GameObject> enemyList = new List<GameObject>();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<EnemyStateMachine>() != null)
                enemyList.Add(child.gameObject);
        }
        enemies = enemyList.ToArray();
    }

    public GameObject CheckEnemyCloseDirect(Vector3 pos)
    {
        CheckEnemies();
        closestObject = null;
        float closestDist = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            EnemyStateMachine sm = enemy.GetComponent<EnemyStateMachine>();
            float navDist = GetNavMeshDistance(enemy.transform.position, pos);

            if (navDist < sm.context.enemyAIData.Range && navDist < closestDist)
            {
                closestObject = enemy;
                closestDist = navDist;
            }
        }
        return closestObject;
    }

    public T[] GetEnemiesRange<T>(Vector3 pos, float range) where T : Component
    {
        CheckEnemies();
        List<T> result = new List<T>();
        foreach (GameObject enemy in enemies)
        {
            float navDist = Vector3.Distance(enemy.transform.position, pos);
            if (navDist > range) continue;

            Vector3 dir = enemy.transform.position - pos;
            if (!Physics.Raycast(pos, dir.normalized, out RaycastHit hit, dir.magnitude)
                || hit.collider.gameObject == enemy)
            {
                T component = enemy.GetComponent<T>();
                if (component != null)
                {
                    Debug.Log(enemy.name);
                    result.Add(component);
                }
            }
        }
        return result.ToArray();
    }

    float GetNavMeshDistance(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path))
        {
            float dist = 0f;
            for (int i = 1; i < path.corners.Length; i++)
                dist += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            return dist;
        }
        return float.MaxValue;
    }
}