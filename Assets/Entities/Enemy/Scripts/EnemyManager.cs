using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private void Awake()
    {
        instance = this;
    }
    
    Audiotary audiotary;

    private void Start()
    {
        audiotary = GetComponent<Audiotary>();
    }

    public void AlertClosestEnemy(Vector3 pos)
    {
        audiotary.CheckEnemyClose(pos).context.events.FireSusEvent();
    }
}