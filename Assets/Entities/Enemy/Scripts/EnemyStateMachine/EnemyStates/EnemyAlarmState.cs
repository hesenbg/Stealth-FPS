using UnityEngine;
using System.Collections;
public class EnemyAlarmState : EnemyState
{
    EnemyStateMachine.EnemyState NextState;

    Vector3 PlayerDirection;

    public enum AlarmedEnemy {Direct, Peek, Flank } // enemy type based on distance.
                                            // if enemy is close, they directly rush to the player.
                                            // if distance is modarete, they take cover and peek.
                                            // if they far away, they go flank
    public AlarmedEnemy Alarmed;


    // direct rusher enemy
    Vector3 LastKnownPlayerPosition;

    // peeker enemy
    Vector3 CoverPos;
    Vector3 PeekDirection;

    // flanker enemy
    Vector3 FlankPos;

    public EnemyAlarmState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        EnemyStateMachine.EnemyState state = EnemyStateMachine.EnemyState.Alarmed;

        return state;
    }

    public override IEnumerator OnStateExit()
    {
        context.enemySight.TargetFullySeen -= OnPlayerSeen;
        yield return null;
    }

    public override IEnumerator OnStateEnter()
    {
        context.enemySight.TargetFullySeen += OnPlayerSeen;
        EnemyManager.instance.DefineAlarmedEnemy(context.parent.transform.position);

        EnemyManager.instance.FindPeekSpot(EnemyManager.instance.LKP,
            context.parent.transform.position, context.enemyAIData.CurrentAwarenessState.SightRange
            , out Vector3 peekPos, out Vector3 peekDirection);

        CoverPos = peekPos;

        PeekDirection = peekDirection;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        LookLKP();
        ProccesAlarmedType();
    }

    private void ProccesAlarmedType()
    {
        switch (Alarmed)
        {
            case AlarmedEnemy.Direct:
                UpdateDirectRusher();
                break;
            case AlarmedEnemy.Peek:
                UpdatePeeker();
                break;
            case AlarmedEnemy.Flank:
                UpdateFlanker(); 
                break;
        }
    }

    private void UpdateDirectRusher()
    {
        context.agent.SetDestination(EnemyManager.instance.LKP);
    }

    private void UpdatePeeker()
    {
         context.agent.SetDestination(CoverPos);    
    }

    private void UpdateFlanker()
    {

    }

    private void LookLKP()
    {
        context.parent.transform.localRotation = Quaternion.LookRotation(PlayerDirection);

    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        PlayerDirection = (context.parent.transform.position- e.GetPos()).normalized;
    }
}