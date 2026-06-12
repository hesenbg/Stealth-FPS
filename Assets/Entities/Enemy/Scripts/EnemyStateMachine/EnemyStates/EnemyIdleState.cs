using System.Collections;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    EnemyStateMachine.EnemyState NextState;
    Vector3[] worldPatrolPositions;
    bool HasCheckAroundEnded = false;

    public EnemyIdleState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey)
        : base(_context, statekey)
    {
        context = _context;
    }

    void TransformLocalToWorld()
    {
        worldPatrolPositions = new Vector3[context.enemyAIData.PatrolPositions.Length];
        for (int i = 0; i < worldPatrolPositions.Length; i++)
            worldPatrolPositions[i] = context.parent.transform.TransformPoint(context.enemyAIData.PatrolPositions[i].Position);
    }

    public override EnemyStateMachine.EnemyState GetNextState() => NextState;

    public override IEnumerator OnStateEnter()
    {

        NextState = StateKey;
        HasCheckAroundEnded = false;
        context.events.SuspiciosEvent += OnSuspiciousEvent;
        context.events.SearchEvent += OnSearchEvent;
        context.events.AlarmEvent += OnAlarmState;


        context.agent.SetDestination(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]);

        while (!context.CheckArrived(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex], 0.2f))
            yield return null;

        HasCheckAroundEnded = true;
    }

    private void OnAlarmState(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    }

    public override IEnumerator OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousEvent;
        context.events.SearchEvent -= OnSearchEvent;
        context.events.AlarmEvent -= OnAlarmState;

        yield return null;
    }

    public override void Init()
    {
        context.healthManager.data = context.enemyAIData;

        TransformLocalToWorld();
        context.enemyAIData.ResetData();
    }

    public override void OnStateUpdate()
    {
        if (context.enemyAIData.IsStill)
            Hold();
        else
            Patrul();
    }

    private void Hold()
    {

    }

    void Patrul()
    {
        if (!HasCheckAroundEnded) return;


        if (context.CheckArrived(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex], 0.2f))
        {
            HasCheckAroundEnded = false;
            float waitTime = context.enemyAIData.PatrolPositions[context.enemyAIData.CurrentPatrolPosIndex].WaitTime;
            if (waitTime > 0)
                context.coreSFM.StartCoroutine(CheckAround(waitTime));
            else
            {
                context.enemyAIData.CurrentPatrolPosIndex = (context.enemyAIData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
                context.agent.SetDestination(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]);
                HasCheckAroundEnded = true;
            }
        }
    }

    IEnumerator CheckAround(float waitTime)
    {
        context.agent.ResetPath();
        yield return new WaitForSeconds(waitTime);
        context.enemyAIData.CurrentPatrolPosIndex = (context.enemyAIData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
        context.agent.SetDestination(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]);
        HasCheckAroundEnded = true;
    }

    private void OnSuspiciousEvent(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Suspicious;
        context.enemyAIData.last.Position = e.GetPos();
    }

    private void OnSearchEvent(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Search;

        context.enemyAIData.CluePosition = e.GetPos();
    }
}
//Debug.Log(sorted[i].e.GetComponentInParent<EnemyStateMachine>().name);
//sorted[i].e.FireClueFound(new EventData(pos, GetDirection(pos)));