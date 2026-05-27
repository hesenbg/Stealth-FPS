using System.Collections;
using UnityEngine;

public class EnemyFightState : EnemyState
{
    public EnemyFightState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return EnemyStateMachine.EnemyState.Fight;
    }

    // variables
    Vector3 PlayerDir;
    Vector3 PlayerPos;

    public override IEnumerator OnStateEnter()
    {
        context.events.FightEvent += OnPlayerSeen;
        context.animationLogic.PlayIdlePistol();
        yield return null;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        PlayerDir = e.GetDir();
        PlayerPos = e.GetPos();
        EnemyManager.instance.LKP = e.GetPos();
    }

    public override IEnumerator OnStateExit()
    {
        context.events.FightEvent -= OnPlayerSeen;
        EnemyManager.instance.IsPlayerInSight = false;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        EnemyManager.instance.IsPlayerInSight = true;

        context.UpdateDirection(PlayerPos);

        context.animationLogic.PlayCrouchPistol();
    }
}