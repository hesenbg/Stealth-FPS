using System;
using UnityEngine;
using static AwarenessFSM;
using static EnemyAlarmState;

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

public struct PeekData
{
    public PeekData(Vector3 coverPos, Vector3 peekDirection)
    {
        CoverPos = coverPos;
        PeekDirection = peekDirection;
    }

    public Vector3 CoverPos;
    public Vector3 PeekDirection;
}

[CreateAssetMenu(menuName = "EnemyStateMachine/Datas")]
public class EnemyAIData : ScriptableObject
{
    [Header("Awareness")]
    public EnemyAwarnesParams CurrentAwarenessState;

    [Header("General")]
    public bool IsStill = false;
    public bool IsProtector = false;

    [Header("Idle State")]
    public PatrolPoint[] PatrolPositions;
    public float IdleSpeed;
    public int CurrentPatrolPosIndex=0;

    [Header("Suspicious State")]
    public float WonderTimer;
    public SuspiciousPoint last;

    [Header("Alarm State")]
    public float LKPchangeTolerance = 3f;
    public float PeekDuration = 1.5f;
    public float TimeBetweenPeeks = 1.25f;
    public float LookAroundAngle = 50f;

    public AlarmedEnemy AlarmedEnemy = AlarmedEnemy.Direct;

    public bool IsHiding = false;
    [Header("Fight State")]
    public PeekData PeekData;
    public PeekEnemy peekPhase;


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