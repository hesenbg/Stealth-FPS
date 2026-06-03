using System.Collections;
using UnityEngine;
public class SniperIdleState : SniperState
{
    public SniperIdleState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }
    SniperStateMachine.SniperState NextState = SniperStateMachine.SniperState.idle;
    Vector3[] worldPatrolPositions;
    bool isWaiting;
    float waitTimer;

    void TransformLocalToWorld()
    {
        worldPatrolPositions = new Vector3[context.GetData.PatrolPositions.Length];
        for (int i = 0; i < worldPatrolPositions.Length; i++)
            worldPatrolPositions[i] = context.GetParent.transform.TransformPoint(context.GetData.PatrolPositions[i].Position);
    }

    public override SniperStateMachine.SniperState GetNextState()
    {
        return NextState;
    }

    public override void Init()
    {
        context.GetHealthManager.data = context.GetData;
    }

    public override IEnumerator OnStateEnter()
    {
        NextState = SniperStateMachine.SniperState.idle;

        context.GetEvents.SuspiciosEvent += OnSuspiciousEvent;
        context.GetEvents.SearchEvent += OnClueFound;
        context.GetEvents.FightEvent += OnPlayerSeen;
        context.GetEvents.AlarmEvent += OnPlayerSeen;

        TransformLocalToWorld();
        context.GetData.CurrentPatrolPosIndex = 0;
        isWaiting = false;
        waitTimer = 0f;
        yield return null;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        NextState = SniperStateMachine.SniperState.Fight;
    }

    private void OnClueFound(object sender, EventData e)
    {
        context.GetData.CluePosition = e.GetPos();
        NextState = SniperStateMachine.SniperState.Search;
    }

    public override IEnumerator OnStateExit()
    {
        context.GetEvents.SuspiciosEvent -= OnSuspiciousEvent;
        context.GetEvents.SearchEvent -= OnClueFound;
        yield return null;
    }

    public override void OnStateUpdate()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                context.GetData.CurrentPatrolPosIndex = (context.GetData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
                isWaiting = false;
            }
            return;
        }

        Vector3 dir = worldPatrolPositions[context.GetData.CurrentPatrolPosIndex] - context.GetParent.transform.position;
        if (context.UpdateRotation(dir, context.GetData.IdleSpeed))
        {
            waitTimer = context.GetData.PatrolPositions[context.GetData.CurrentPatrolPosIndex].WaitTime;
            isWaiting = true;
        }
    }

    private void OnSuspiciousEvent(object sender, EventData e)
    {
        NextState = SniperStateMachine.SniperState.Suspicious;
        context.GetData.last.Position = e.GetPos();
    }
}