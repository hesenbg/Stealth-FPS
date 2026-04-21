using System.Collections;
using UnityEngine;
using System;
public class EnemySuspiciousState : EnemyState
{
    enum InvestigationPhase { Turning, Navigating, Investigating, Done }
    InvestigationPhase phase;

    public EnemySuspiciousState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        if (phase == InvestigationPhase.Done)
            return EnemyStateMachine.EnemyState.Idle;
        return StateKey;
    }

    public override void Init() { }

    public override IEnumerator OnStateEnter()
    {
        context.events.SuspiciosEvent += OnSuspiciousTargetOnSight; ;
        ResetState();
        yield break;
    }

    public override IEnumerator OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousTargetOnSight; ;
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
                break;
        }
    }

    public override void OnStateFixedUpdate() { }

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