using System;
using UnityEngine;

public class EventData : EventArgs
{
    Vector3 position;
    Vector3 direction;
    public EventData(Vector3 dir)
    {
        direction = dir;
        position = Vector3.zero;
    }
    public EventData(Vector3 pos,Vector3 dir)
    {
        position = pos;
        direction = dir;
    }
    public EventData()
    {
    }
    public bool IsPosAvalible()
    {
        return position== Vector3.zero;
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

public class SuspiciousData : EventData
{

}

public class EnemyEvents : MonoBehaviour
{
    [SerializeField] VisionCone sight;

    public event EventHandler <EventData> SuspiciosEvent;  

    public event EventHandler <EventData> AlarmEvent;

    public event EventHandler<EventData> SearchEvent;

    public event EventHandler<EventData> FightEvent;

    private void Start()
    {
        sight.TargetSuspiciousSight += OnSuspiciousEvent;
        sight.TargetAnomalySeen += OnClueFound;
        sight.TargetFullySeen += OnTargetSpotted;
    }

    private void OnTargetSpotted(object sender, EventData e)
    {
        FirePlayerSeen(e.GetPos());
        Debug.Log("full seen");
    }

    private void OnClueFound(object sender, EventData e)
    {
        FireClueFound(e.GetPos());
        Debug.Log("clue seen");
    }

    private void OnSuspiciousEvent(object sender, EventData e)
    {
        FireSusEvent(e.GetPos());
        Debug.Log("Sus seen");
    }

    public void FireClueFound(Vector3 pos)
    {
        SearchEvent?.Invoke(this, new EventData(pos,(transform.position-pos).normalized));
    }

    public void FirePlayerSeen(Vector3 pos)
    {
        FightEvent?.Invoke(this, new  EventData(pos, (transform.position - pos).normalized));
    }

    public void FireSusEvent(Vector3 pos)
    {
        SuspiciosEvent?.Invoke(this, new EventData(pos, (transform.position - pos).normalized));
    }
}