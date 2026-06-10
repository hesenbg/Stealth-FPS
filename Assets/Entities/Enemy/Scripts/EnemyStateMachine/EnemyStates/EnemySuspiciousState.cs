using System.Collections;
using UnityEngine;
using System;
public class EnemySuspiciousState : EnemyState
{
    enum InvestigationPhase { Turning, Navigating, Investigating, Done }
    public enum SuspiciousDegree { Glance, Investigate, Search }
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

    public override void Init()
    {
        degree = SuspiciousDegree.Glance;
    }

    public override IEnumerator OnStateEnter()
    {
        context.events.SuspiciosEvent += OnSuspiciousTargetOnSight;
        context.events.SearchEvent += OnClueFound;
        context.events.FightEvent += OnPlayerSeen;
        context.events.AlarmEvent += OnAlarmEvent;

        NextState = EnemyStateMachine.EnemyState.Suspicious;

        ResetState();
        yield break;
    }

    private void OnAlarmEvent(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    }

    public override IEnumerator OnStateExit()
    {
        context.agent.updateRotation = true;

        context.events.SuspiciosEvent -= OnSuspiciousTargetOnSight;
        context.events.SearchEvent -= OnClueFound;
        context.events.FightEvent -= OnPlayerSeen;
        context.events.AlarmEvent -= OnAlarmEvent;

        context.agent.ResetPath();
        context.enemyAIData.ResetData();
        yield break;
    }

    public override void OnStateUpdate()
    {
        Debug.Log(phase);

        switch (phase)
        {
            case InvestigationPhase.Turning:
                if (context.UpdateDirection(context.enemyAIData.last.Position))
                {
                    context.agent.updateRotation = true;
                    phase = InvestigationPhase.Navigating;
                }
                break;
            case InvestigationPhase.Navigating:
                context.agent.SetDestination(context.enemyAIData.last.Position);
                if (context.CheckArrived(context.enemyAIData.last.Position, 0.8f))
                {
                    context.agent.updateRotation = false;
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
        yield return context.coreSFM.StartCoroutine(context.LookAround(context.parent.transform.forward,
            context.enemyAIData.WonderTimer,
            45f,
            context.enemyAIData.InterplationSpeed));
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
        NextState = EnemyStateMachine.EnemyState.Alarmed;
        EnemyManager.instance.LKP = e.GetPos();
        context.events.FireAlarm(e);
        EnemyManager.instance.CallAlliesOnAlarm();
    }

    private void OnClueFound(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Search;
        context.enemyAIData.CluePosition = e.GetPos();
    }
}