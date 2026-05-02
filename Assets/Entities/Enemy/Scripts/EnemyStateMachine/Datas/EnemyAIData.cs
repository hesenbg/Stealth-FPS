using UnityEngine;
using System;
[Serializable]
public struct EnemyAwarnesParams
{
    public float Range;
    public float AwarenessSpeed;
    public float Speed;
    public float AroundCheckAngle;
    public float AroundCheckSpeed;
    public float AroundCheckDelay;
}
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

    [Header("data")]
    public float Range;
    public float InterplationSpeed;
    public float BulletHearMaxAngle = 30f;

    [Header("Awareness effects")]
    public float CurrentAwareness;
    public EnemyAwarnesParams Idle;
    public EnemyAwarnesParams Suspicious;
    public EnemyAwarnesParams Alarmed;
    [HideInInspector] public EnemyAwarnesParams current;

    public void ResetData()
    {
        last.IsActive = false;
        last.Position = Vector3.zero;
    }
}