using System;
using System.Collections.Generic;
using UnityEngine;
public class Audiotary : MonoBehaviour
{
    GameObject[] enemies;

    public event EventHandler soundGive;

    GameObject closestObject;

    private void Awake()
    {
        List<GameObject> enemyList = new List<GameObject>();

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<EnemyStateMachine>() != null)
                enemyList.Add(child.gameObject);
        }

        enemies = enemyList.ToArray();
    }

    public void CheckEnemyClose(Vector3 pos, float range)
    {
        foreach (GameObject enemy in enemies)
        {
            EnemyStateMachineContext cont = enemy.GetComponent<EnemyStateMachine>().context;

            if(Vector3.Distance(enemy.transform.position, pos) < range)
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
    }
}