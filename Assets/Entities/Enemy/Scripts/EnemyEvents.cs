using System;
using UnityEngine;
public class SightData : EventArgs
{
    public Vector3 Position;
    public SightData(Vector3 pos)
    {
        Position = pos;
    }

    public SightData() { }
}

public class SuspiciousData : EventArgs
{
    public Vector3 Position;
    public SuspiciousData(Vector3 pos)
    {
        Position = pos;
    }
}
public class EnemyEvents : MonoBehaviour
{
    [SerializeField] VisionCone sight;

    public event EventHandler <SuspiciousData> SuspiciosEvent;

    public event EventHandler <SightData> PlayerSeen;

    public event EventHandler <SightData> ClueFound;

    private void Start()
    {
        sight.TargetSuspiciousSight += OnTargetSuspiciousSight;
        sight.TargetAnomalySeen += OnTargetAnomalySeen;
        sight.TargetFullySeen += OnTargetFullySeen;
    }

    private void OnTargetFullySeen(object sender, SightData e)
    {
        FirePlayerSeen(e.Position);
    }

    private void OnTargetAnomalySeen(object sender, SightData e)
    {
        FireClueFound(e.Position);
    }

    private void OnTargetSuspiciousSight(object sender, SightData e)
    {
        FireSusEvent(e.Position);
    }

    public void FireClueFound(Vector3 pos)
    {
        ClueFound?.Invoke(this, new SightData(pos));
    }

    public void FirePlayerSeen(Vector3 pos)
    {
        PlayerSeen?.Invoke(this, new  SightData(pos));
    }

    public void FireSusEvent(Vector3 pos)
    {
        SuspiciosEvent?.Invoke(this, new SuspiciousData(pos));
    }
}