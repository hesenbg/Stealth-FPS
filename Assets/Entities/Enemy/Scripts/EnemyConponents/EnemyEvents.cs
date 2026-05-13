using System;
using UnityEngine;

public class EventData : EventArgs
{
    Vector3 position;
    Vector3 direction;

    public EventData(Vector3 dir)
    {
        direction = dir;
        position = dir * 4.5f;
    }

    public EventData(Vector3 pos, Vector3 dir)
    {
        position = pos;
        direction = dir;
    }

    public EventData()
    {
    }

    public Vector3 GetPos()
    {
        return position;
    }

    public Vector3 GetDir()
    {
        return direction;
    }
}

public class EnemyEvents : MonoBehaviour
{
    [SerializeField] VisionCone sight;

    EnemyStateMachineContext context;

    public event EventHandler<EventData> SuspiciosEvent;

    public event EventHandler<EventData> AlarmEvent;

    public event EventHandler<EventData> SearchEvent;

    public event EventHandler<EventData> FightEvent;

    private void Start()
    {
        context = GetComponentInParent<EnemyStateMachine>().context;
        sight = context.enemySight;

        sight.TargetSuspiciousSight += OnSuspiciousEvent;
        sight.TargetAnomalySeen += OnClueFound;
        sight.TargetFullySeen += OnTargetSpotted;
    }

    private void OnTargetSpotted(object sender, EventData e)
    {
        FirePlayerSeen(e);
        Debug.Log("full seen");
    }

    private void OnAlarmEvent(object sender, EventData e)
    {
        FireAlarm(e);
        Debug.Log("Alarm event");
    }

    private void OnClueFound(object sender, EventData e)
    {
        FireClueFound(e);
        Debug.Log("clue seen");
    }

    private void OnSuspiciousEvent(object sender, EventData e)
    {
        FireSusEvent(e);
        Debug.Log("Sus seen");
    }

    public void FireClueFound(EventData data)
    {
        Debug.Log("Firing Clue Found Event at: " );
        SearchEvent?.Invoke(this, data);
    }

    public void FirePlayerSeen(EventData data)
    {
        Debug.Log("Firing Player Seen Event at: " );
        FightEvent?.Invoke(this, data);
    }

    public void FireAlarm(EventData data)
    {
        Debug.Log("Firing Alarm Event at: " );
        AlarmEvent?.Invoke(this, data);
    }

    public void FireSusEvent(EventData data)
    {
        Debug.Log("Firing Suspicious Event at: ");
        SuspiciosEvent?.Invoke(this, data);
        context.enemyAIData.last.Position = data.GetPos();
    }
}