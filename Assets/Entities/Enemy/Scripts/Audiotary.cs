using System;
using System.Collections.Generic;
using UnityEngine;
public class Audiotary : MonoBehaviour
{
    EnemyStateMachine[] enemies;

    public event EventHandler soundGive;

    EnemyStateMachine closestObject;

    private void Awake()
    {
        List<EnemyStateMachine> enemyList = new List<EnemyStateMachine>();

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<EnemyStateMachine>() != null)
                enemyList.Add(child.gameObject.GetComponent<EnemyStateMachine>());
        }

        enemies = enemyList.ToArray();
    }

    public EnemyStateMachine CheckEnemyClose(Vector3 pos)
    {
        closestObject = null;
        float closestDist = float.MaxValue;

        foreach (EnemyStateMachine enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, pos);
            if (dist < enemy.context.enemyAIData.Range && dist < closestDist)
            {
                Vector3 direction = (enemy.transform.position - pos).normalized;
                if (Physics.Raycast(pos, direction, out RaycastHit hit, dist + 0.5f))
                {
                    if (hit.collider.gameObject == enemy.gameObject)
                    {
                        closestObject = enemy;
                        closestDist = dist;
                    }
                }
            }
        }
        return closestObject;
    }
}