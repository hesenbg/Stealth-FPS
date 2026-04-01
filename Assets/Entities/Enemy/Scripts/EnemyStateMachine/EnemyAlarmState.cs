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
        EnemyStateMachine.EnemyState state = EnemyStateMachine.EnemyState.Alarmed;

        return state;
    }
    public override void Init()
    {
        
    }


    public override void OnStateEnter()
    {
        Debug.Log("Alarm state");
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
