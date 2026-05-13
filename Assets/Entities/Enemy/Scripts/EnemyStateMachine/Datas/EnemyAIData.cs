using System;
using UnityEngine;
using static AwarenessFSM;
[Serializable]

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
[Serializable]
public struct PatrolPoint
{
    public Vector3 Position;
    public float WaitTime;
}

[CreateAssetMenu(menuName = "EnemyStateMachine/Datas")]
public class EnemyAIData : ScriptableObject
{
    [Header("Awareness")]
    public EnemyAwarnesParams CurrentAwarenessState;

    [Header("General")]
    public bool IsStill = false;

    [Header("Idle State")]
    public PatrolPoint[] PatrolPositions;
    public float IdleSpeed;
    public int CurrentPatrolPosIndex=0;

    [Header("Suspicious State")]
    public float WonderTimer;
    public SuspiciousPoint last;

    [Header("Alarm State")]

    [Header("Search State")]
    public Vector3 CluePosition;

    public float InterplationSpeed;

    [Header("Communication")]
    public float AllyCallDelay;
    public int CalledAllyNumber;

    public void ResetData()
    {
        last.IsActive = false;
        last.Position = Vector3.zero;
        CluePosition = Vector3.zero;
    }
}