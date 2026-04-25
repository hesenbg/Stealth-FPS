using System.Collections;
using UnityEngine;
using System;
public class EnemySuspiciousState : EnemyState
{
    enum InvestigationPhase { Turning, Navigating, Investigating, Done }
    InvestigationPhase phase;

    EnemyStateMachine.EnemyState NextState;

    public EnemySuspiciousState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        if (phase == InvestigationPhase.Done)
            return NextState;
        return NextState;
    }

    public override void Init() { }

    public override IEnumerator OnStateEnter()
    {
        context.events.SuspiciosEvent += OnSuspiciousTargetOnSight;
        context.events.ClueFound += OnClueFound;
        context.events.PlayerSeen += OnPlayerSeen;

        NextState = EnemyStateMachine.EnemyState.Suspicious;

        ResetState();
        yield break;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Fight;
    }

    private void OnClueFound(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousTargetOnSight;
        context.agent.ResetPath();
        yield break;
    }

    public override void OnStateUpdate()
    {
        switch (phase)
        {
            case InvestigationPhase.Turning:
                if (context.UpdateDirection(context.enemyAIData.last.Position))
                    phase = InvestigationPhase.Navigating;
                break;

            case InvestigationPhase.Navigating:
                context.agent.SetDestination(context.enemyAIData.last.Position);
                if (context.CheckArrived(context.enemyAIData.last.Position, 1f))
                {
                    phase = InvestigationPhase.Investigating;
                    context.coreSFM.StartCoroutine(Investigate());
                }
                break;

            case InvestigationPhase.Investigating:
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
}