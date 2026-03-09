using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
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
        Debug.Log("exit from idle state");
    }

    public override void OnStateUpdate()
    {
    }
}
