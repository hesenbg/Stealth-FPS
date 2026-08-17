using System.Collections;
using UnityEngine;

public class SniperFightState : SniperState
{
    public SniperFightState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }

    Vector3 PlayerPos = Vector3.zero;

    SniperStateMachine.SniperState nextState = SniperStateMachine.SniperState.Fight;

    public override SniperStateMachine.SniperState GetNextState()
    {
        return nextState;
    }

    public override IEnumerator OnStateEnter()
    {
        context.GetEvents.FightEvent += OnPlayerSeen;

        yield return null;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        PlayerPos = e.GetPos();
        EnemyManager.instance.LKP = e.GetPos();
    }

    public override IEnumerator OnStateExit()
    {
        context.GetEvents.FightEvent -= OnPlayerSeen;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        Vector3 DirectionToPlayer = (EnemyManager.instance.LKP - context.GetParent.transform.position).normalized;

        context.UpdateRotation( DirectionToPlayer , 6f);
        if (context.GetEnemyCombatLogic.CanShoot())
        {
            context.GetEnemyCombatLogic.Shoot(EnemyManager.instance.LKP,EnemyType.Sniper);
        }
    }
}