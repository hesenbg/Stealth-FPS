using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchState : EnemyState
{
    public EnemySearchState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    List<Vector3> covers;
    int coverIndex;
    bool ready;
    bool done;

    EnemyStateMachine.EnemyState NextState = EnemyStateMachine.EnemyState.Search;

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return NextState;
    }

    public override void Init() { }

    public override IEnumerator OnStateEnter()
    {
        context.events.FightEvent += OnPlayerSeen;
        context.events.AlarmEvent += OnAlarmState;
        context.events.SearchEvent += OnSearchEvent;
        context.enemySight.TargetSuspiciousSight += OnSuspiciousTargetSeen;

        NextState = EnemyStateMachine.EnemyState.Search;

        ready = false;
        done = false;

        covers = EnemyManager.instance.GenerateCoverPos(3, 8f, context.parent.transform.position);

        covers.Insert(0, context.enemyAIData.CluePosition);

        coverIndex = 0;

        yield return new WaitForSeconds(1.5f);

        ready = true;
        SetNextDestination();
    }

    private void OnSuspiciousTargetSeen(object sender, EventData e)
    {
        covers.Insert(0, e.GetPos());
    }

    private void OnSearchEvent(object sender, EventData e)
    {
        covers.Insert(0, e.GetPos());
    }

    private void OnAlarmState(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        EnemyManager.instance.LKP = e.GetPos();
        NextState = EnemyStateMachine.EnemyState.Alarmed;
        context.events.FireAlarm(e);
        EnemyManager.instance.CallAlliesOnAlarm(context.events);
    }

    public override IEnumerator OnStateExit()
    {
        context.events.FightEvent -= OnPlayerSeen;
        context.events.AlarmEvent -= OnAlarmState;
        context.events.SearchEvent -= OnSearchEvent;
        context.enemySight.TargetSuspiciousSight -= OnSuspiciousTargetSeen;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        Vector3 DirectionToPlayer = (EnemyManager.instance.LKP - context.parent.transform.position).normalized;

        if (!ready) return;

        if (context.CheckArrivedSight(covers[coverIndex], 0.5f))
        {
            coverIndex++;

            if (coverIndex >= covers.Count)
            {
                done = true;
                NextState = EnemyStateMachine.EnemyState.Idle;
                return;
            }

            SetNextDestination();
        }
    }

    void SetNextDestination()
    {
        context.agent.SetDestination(covers[coverIndex]);
    }
}