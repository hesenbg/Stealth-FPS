using System;
using UnityEngine;
public class Audiotary : MonoBehaviour
{
    GameObject[] enemies;

    public event EventHandler soundGive;

    GameObject closestObject;

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



    }
}