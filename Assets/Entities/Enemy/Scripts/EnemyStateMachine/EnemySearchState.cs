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

    public override void OnStateEnter()
    {

    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {

    }
}