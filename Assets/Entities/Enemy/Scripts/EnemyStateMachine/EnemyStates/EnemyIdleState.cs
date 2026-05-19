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
        if(context.enemyAIData.IsStill)
            yield break;
        context.agent.SetDestination(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]);
        context.animationLogic.PlayWalk();

        while (!context.CheckArrived(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex], 0.2f))
            yield return null;

        HasCheckAroundEnded = true;
    }

    public override IEnumerator OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousEvent;
        context.events.SearchEvent -= OnSearchEvent;

        yield return null;
    }

    public override void Init()
    {
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

    void Patrul()
    {
        if (!HasCheckAroundEnded) return;

        context.animationLogic.PlayWalk();

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

    void Hold()
    {
        context.animationLogic.PlayIdlePistol();
    }

    IEnumerator CheckAround(float waitTime)
    {
        context.agent.ResetPath();
        context.animationLogic.PlayIdleLookAround();
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