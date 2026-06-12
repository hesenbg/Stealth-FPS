using System;
using UnityEngine;

public class EventData : EventArgs
{
    Vector3 position;
    Vector3 direction;

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
}

public class EnemyEvents : MonoBehaviour
{
    VisionCone sight;

    public event EventHandler<EventData> SuspiciosEvent;

    public event EventHandler<EventData> AlarmEvent;

    public event EventHandler<EventData> SearchEvent;

    public event EventHandler<EventData> FightEvent;

    public enum EnemyType { Guard, Protector, Sniper}

    public EnemyType Type;

    private void Start()
    {
        sight = GetComponent<VisionCone>();
        
        sight.TargetSuspiciousSight += OnSuspiciousEvent;
        sight.TargetAnomalySeen += OnClueFound;
        sight.TargetFullySeen += OnTargetSpotted;

        Getdata();
    }

    public EnemyAIData Getdata()
    {
        EnemyStateMachine esm = GetComponentInParent<EnemyStateMachine>();
        if (esm != null) return esm.context.enemyAIData;

        SniperStateMachine ssm = GetComponentInParent<SniperStateMachine>();
        if (ssm != null) return ssm.context.GetData;

        Debug.LogError($"No state machine found on {transform.root.name}");
        return null;
    }



    private void OnTargetSpotted(object sender, EventData e)
    {
        //Debug.Log("full seen");
        FirePlayerSeen(e);
    }

    private void OnAlarmEvent(object sender, EventData e)
    {
        //Debug.Log("Alarm event");
        FireAlarm(e);
    }

    private void OnClueFound(object sender, EventData e)
    {
        //Debug.Log("clue seen");
        FireClueFound(e);
    }

    private void OnSuspiciousEvent(object sender, EventData e)
    {
        //Debug.Log("Sus seen");
        FireSusEvent(e);
    }

    public void FireClueFound(EventData data)
    {
        //Debug.Log("Firing Clue Found Event at: " );
        //SearchEvent?.Invoke(this, data);

        EnemyManager.instance.CallAlliesOnClue(data.GetPos(), 2, this);
    }

    public void FireSearchState(EventData data)
    {
        SearchEvent?.Invoke(this, data);
    }

    public void FirePlayerSeen(EventData data)
    {
        //Debug.Log("Firing Player Seen Event at: " );
        FightEvent?.Invoke(this, data);
    }

    public void FireAlarm(EventData data)
    {
        //Debug.Log("Firing Alarm Event at: " );
        AlarmEvent?.Invoke(this, data);
    }

    public void FireSusEvent(EventData data)
    {
        //Debug.Log("Firing Suspicious Event at: ");
        SuspiciosEvent?.Invoke(this, data);
    }
}