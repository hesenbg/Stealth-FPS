using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    HealthManager[] EnemyHealths;

    private void Awake()
    {
        instance = this;
        EnemyHealths = GetComponentsInChildren<HealthManager>();
    }
    
    Audiotary audiotary;

    private void Start()
    {
        audiotary = GetComponent<Audiotary>();
    }

    public void AlertClosestEnemy(Vector3 pos)
    {
        EnemyStateMachine detected =  audiotary.CheckEnemyClose(pos);
        if(detected != null)
        {
            //Debug.Log(detected.gameObject.name);    
            detected.context.events.FireSusEvent();
            detected.context.enemyAIData.last.SetValue(pos);
        } 
    }
}