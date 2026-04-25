using UnityEngine;
using System.Collections;

public class EnemyAlarmState : EnemyState
{
    EnemyStateMachine.EnemyState NextState;

    Vector3 PlayerDirection;

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

    public override IEnumerator OnStateExit()
    {
        context.enemySight.TargetFullySeen -= OnPlayerSeen;
        yield return null;
    }

    public override IEnumerator OnStateEnter()
    {
        context.enemySight.TargetFullySeen += OnPlayerSeen;
        yield return null;
    }

    public override void OnStateUpdate()
    {
        context.parent.transform.localRotation = Quaternion.LookRotation(PlayerDirection);
    }

    private void OnPlayerOutSite(object sender, EventData data)
    {
        NextState = EnemyStateMachine.EnemyState.Search;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        PlayerDirection = (context.parent.transform.position- e.GetPos()).normalized;
    }

    private void OnPlayerInSight(object sender, EventData data)
    {

    }
}