using System.Collections;
using UnityEngine;
using System;
public class EnemySuspiciousState : EnemyState
{
    enum InvestigationPhase { Turning, Navigating, Investigating, Done }
    public enum SuspiciousDegree { Glance, Investigate, Search } // based on how important is the suspicous event.
                                                                 // if event is serious or enenmy saw multiple of them it goes to search state
    InvestigationPhase phase;
    SuspiciousDegree degree;

    EnemyStateMachine.EnemyState NextState;

    public EnemySuspiciousState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        if (phase == InvestigationPhase.Done)
            return EnemyStateMachine.EnemyState.Idle;
        return NextState;
    }

    public override void Init() {
        degree = SuspiciousDegree.Glance;
    }

    public override IEnumerator OnStateEnter()
    {
        context.events.SuspiciosEvent += OnSuspiciousTargetOnSight;
        context.events.SearchEvent += OnClueFound;
        context.events.FightEvent += OnPlayerSeen;

        NextState = EnemyStateMachine.EnemyState.Suspicious;

        ResetState();
        yield break;
    }
    public override IEnumerator OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousTargetOnSight;
        context.events.SearchEvent -= OnClueFound;
        context.events.FightEvent -= OnPlayerSeen;
        context.agent.ResetPath();

        context.enemyAIData.ResetData();
        yield break;
    }

    public override void OnStateUpdate()
    {
        switch (phase)
        {
            case InvestigationPhase.Turning:
                if (context.UpdateDirection(context.enemyAIData.last.Position))
                {
                    phase = InvestigationPhase.Navigating;
                    context.animationLogic.PlayIdle();
                }  
                break;

            case InvestigationPhase.Navigating:
                context.agent.SetDestination(context.enemyAIData.last.Position);
                context.animationLogic.PlayWalk();
                if (context.CheckArrived(context.enemyAIData.last.Position, 0.8f))
                {
                    phase = InvestigationPhase.Investigating;
                    context.coreSFM.StartCoroutine(Investigate());
                }
                break;

            case InvestigationPhase.Investigating:
                context.ResetVelocity();
                break;
            case InvestigationPhase.Done:
                NextState = EnemyStateMachine.EnemyState.Idle;
                break;
        }
    }

    IEnumerator Investigate()
    {
        context.animationLogic.PlayIdleLookAround();
        yield return new WaitForSeconds(context.enemyAIData.WonderTimer);
        context.animationLogic.PlayWalk();
        phase = InvestigationPhase.Done;
    }
        
    private void ResetState()
    {
        phase = InvestigationPhase.Turning;
        context.agent.enabled = true;
        context.agent.ResetPath();
    }

    private void OnSuspiciousTargetOnSight(object sender, EventArgs e)
    {
        ResetState();
    }
    private void OnPlayerSeen(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Fight;
    }

    private void OnClueFound(object sender, EventArgs e)
    {
        NextState = EnemyStateMachine.EnemyState.Search;
    }
}