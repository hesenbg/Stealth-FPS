using UnityEngine;
[System.Serializable]
public struct EnemyAwarnesParams
{
    public float Range;
    public float AwarenessSpeed;
    public float Speed;
    public float AroundCheckAngle;
    public float AroundCheckSpeed;
    public float AroundCheckDelay;
}
[System.Serializable]
public struct SuspiciousPoint
{
    public void SetValue( Vector3 pos)
    {
        Position = pos;
        IsActive = true;
    }

    public void Reset()
    {
        IsActive = false;
    }

    public Vector3 Position;
    public bool IsActive;
}


[CreateAssetMenu(menuName = "EnemyStateMachine/Datas")]
public class EnemyAIData : ScriptableObject
{
    [Header("Idle State")]
    public Vector3[] PatrolPositions;
    public float IdleSpeed;

    [Header("Suspicious State")]
    public float WonderTimer;
    public SuspiciousPoint last;

    [Header("Alarm State")]

    [Header("data")]
    public float Range;
    public float InterplationSpeed;

    [Header("Awareness effects")]
    public EnemyAwarnesParams Idle;
    public EnemyAwarnesParams Suspicious;
    public EnemyAwarnesParams Alarmed;

    [HideInInspector] public EnemyAwarnesParams current;
}