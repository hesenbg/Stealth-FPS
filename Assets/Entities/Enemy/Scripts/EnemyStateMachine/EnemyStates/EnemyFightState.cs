using System.Collections;
using UnityEngine;

public class EnemyFightState : EnemyState
{
    public EnemyFightState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }
    EnemyStateMachine.EnemyState NextState = EnemyStateMachine.EnemyState.Fight; 

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return NextState;
    }

    // variables
    Vector3 PlayerDir;
    Vector3 PlayerPos;

    public override IEnumerator OnStateEnter()
    {
        NextState = EnemyStateMachine.EnemyState.Fight;

        context.events.FightEvent += OnPlayerSeen;
        context.enemySight.TargetoutSight += OnTargetOutSite;
        context.animationLogic.PlayIdlePistol();
        yield return null;
    }

    private void OnTargetOutSite(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    }

    bool IsUpdated = false;

    private void OnPlayerSeen(object sender, EventData e)
    {
        IsUpdated = true;
        PlayerPos = e.GetPos();
        PlayerDir = (PlayerPos - context.parent.transform.position).normalized;
        EnemyManager.instance.LKP = e.GetPos();

        context.events.FireAlarm(new EventData(EnemyManager.instance.LKP, PlayerDir));
    }

    public override IEnumerator OnStateExit()
    {
        context.events.FightEvent -= OnPlayerSeen;
        EnemyManager.instance.IsPlayerInSight = false;
        IsUpdated  = false;
        yield return null;
    }

    public override void OnStateUpdate()
    {
        EnemyManager.instance.IsPlayerInSight = true;

        if(IsUpdated)
            context.UpdateDirection(EnemyManager.instance.LKP);

        if (context.enemyCombat.CanShoot())
        {
            context.enemyCombat.Shoot((PlayerPos - context.parent.transform.position));
        }

        context.animationLogic.PlayIdlePistol();
    }
}