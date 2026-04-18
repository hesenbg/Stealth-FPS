using System.Collections;
using UnityEngine;

public class EnemySearchState : EnemyState
{
    public EnemySearchState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        EnemyStateMachine.EnemyState state = EnemyStateMachine.EnemyState.Idle;

        return state;
    }

    public override void Init()
    {
        
    }

    public override IEnumerator OnStateEnter()
    {
        yield return null;

    }

    public override IEnumerator OnStateExit()
    {
        yield return null;

    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateFixedUpdate()
    {

    }
}