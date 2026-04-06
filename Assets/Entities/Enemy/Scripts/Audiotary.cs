using System;
using System.Collections.Generic;
using UnityEngine;
public class Audiotary : MonoBehaviour
{
    public event EventHandler soundGive;

    Audiotary audiotary;

    private void Start()
    {
        audiotary = GetComponent<Audiotary>();
    }

    public void AlertClosestEnemy(Vector3 pos)
    {
        EnemyStateMachine detected = EnemyManager.instance.CheckEnemyCloseDirect(pos).GetComponent<EnemyStateMachine>();
        if (detected != null)
        {
            //Debug.Log(detected.gameObject.name);    
            detected.context.events.FireSusEvent();
            detected.context.enemyAIData.last.SetValue(pos);
        }
    }
}