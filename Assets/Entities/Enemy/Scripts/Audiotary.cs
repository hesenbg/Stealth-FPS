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
        foreach (EnemyStateMachine enemy in enemies)
        {
            EnemyStateMachineContext cont = enemy.GetComponent<EnemyStateMachine>().context;

            if(Vector3.Distance(enemy.transform.position, pos) < enemy.context.enemyAIData.Range)
            {
                Vector3 direction = (enemy.transform.position- pos).normalized;

                float distance = Mathf.Abs(Vector3.Distance(enemy.transform.position, pos));

                if (Physics.Raycast(pos, direction, out RaycastHit hit, distance + 0.5f))
                {
                    if(hit.collider.gameObject == enemy)
                        closestObject = enemy;
                }
            }
        }
        Debug.Log(closestObject.name);
        return closestObject;
    }
}