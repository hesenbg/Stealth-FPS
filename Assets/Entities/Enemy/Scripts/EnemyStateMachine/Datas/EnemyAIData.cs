using UnityEngine;

[CreateAssetMenu(menuName ="EnemyStateMachine/Datas")]
public class EnemyAIData : ScriptableObject
{
    [Header("Idle State")]
    public Vector3[] PatrolPositions;
    public float IdleSpeed;

    [Header("Suspicious State")]
    public float WonderTimer;
    public float SuspiciousState;



    [Header("data")]
    public float Range;
}