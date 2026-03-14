using UnityEngine;

public abstract class EnemyState : BaseState<EnemyStateMachine.EnemyState>
{
    protected EnemyStateMachineContext context;

    public EnemyState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(statekey)
    {
        context = _context;
    }
}