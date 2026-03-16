using UnityEngine;

public class EnemyAlarmState : EnemyState
{
    EnemyStateMachine.EnemyState NextState;

    public EnemyAlarmState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
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
        //context.enemySight.TargetSuspiciousSight += OnEnemyInSight;
        //context.enemySight.TargetoutSight += OnEnemyOutSite;
    }

    private void OnEnemyOutSite(object sender, System.EventArgs e)
    {
        NextState = EnemyStateMachine.EnemyState.Search;
    }

    private void OnEnemyInSight(object sender, System.EventArgs e)
    {

    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {

    }
}
