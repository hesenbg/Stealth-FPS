using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        context.events.AlarmEvent += OnAlarmState; ;


        NextState = EnemyStateMachine.EnemyState.Search;

        ready = false;
        done = false;
        covers = EnemyManager.instance.GenerateCoverPos(3, 6.5f, context.parent.transform.position);
        coverIndex = 0;

        covers.Insert(0,context.enemyAIData.CluePosition);

        yield return new WaitForSeconds(1.5f);
        ready = true;
        SetNextDestination();
    }

    private void OnAlarmState(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        EnemyManager.instance.LKP = e.GetPos();
        NextState = EnemyStateMachine.EnemyState.Fight;
        context.events.FireAlarm(e);
        EnemyManager.instance.AlertAllies();
    }

    public override IEnumerator OnStateExit()
    {
        context.events.FightEvent -= OnPlayerSeen;
        context.events.AlarmEvent -= OnAlarmState; ;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        if (!ready) return;

        context.animationLogic.PlayWalkPistol();

        if (context.CheckArrived(covers[coverIndex], 0.5f))
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