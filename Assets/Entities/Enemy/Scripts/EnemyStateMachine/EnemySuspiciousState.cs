using UnityEngine;

public class EnemySuspiciousState : EnemyState
{
    public EnemySuspiciousState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return EnemyStateMachine.EnemyState.Suspicious;
    }


    public override void OnStateEnter()
    {
        context.agent.SetDestination(context.enemyAIData.last.Position);
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {

    }
}
