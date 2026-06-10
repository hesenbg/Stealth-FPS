using UnityEngine;
using System.Collections;

public class EnemyAlarmState : EnemyState
{
    EnemyStateMachine.EnemyState NextState = EnemyStateMachine.EnemyState.Alarmed;

    public enum AlarmedEnemy { Direct, Peek }
    public AlarmedEnemy Alarmed;

    public enum PeekEnemy { Procces, Cover, Peek }

    bool IsPeekAble = false;
    Vector3 PrevLKP = Vector3.zero;

    public EnemyAlarmState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return NextState;
    }

    public override void Init()
    {
        PrevLKP = EnemyManager.instance.LKP;
    }

    public override IEnumerator OnStateExit()
    {
        context.enemySight.TargetFullySeen -= OnPlayerSeen;
        context.enemyAIData.IsHiding = false;
        context.agent.updateRotation = true;
        context.agent.ResetPath();

        yield return null;
    }

    public override IEnumerator OnStateEnter()
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
        context.enemyAIData.IsHiding = false;

        UpdateRole();

        context.agent.updateRotation = false;
        context.enemySight.TargetFullySeen += OnPlayerSeen;

        AssignPeekParams();

        if (Alarmed == AlarmedEnemy.Peek && !IsPeekAble)
            Alarmed = AlarmedEnemy.Direct;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        LookLKP();

        context.agent.SetDestination(EnemyManager.instance.LKP);

        if(context.CheckArrived(EnemyManager.instance.LKP, 0.2f))
        {
            NextState = EnemyStateMachine.EnemyState.Search;
        }
    }

    void UpdateRole()
    {
        Alarmed = EnemyManager.instance.DefineAlarmedEnemy(context.parent.transform.position);

        context.enemyAIData.AlarmedEnemy = Alarmed;
        if (Vector3.Distance(PrevLKP, EnemyManager.instance.LKP) >= context.enemyAIData.LKPchangeTolerance)
        {
        }
    }

    void AssignPeekParams()
    {
        IsPeekAble = EnemyManager.instance.FindPeekSpot(EnemyManager.instance.LKP,
            context.parent.transform.position, context.enemyAIData.CurrentAwarenessState.SightRange,
            out Vector3 peekPos, out Vector3 peekDirection);

        context.enemyAIData.CoverPos  = peekPos;
        context.enemyAIData.PeekDirection  = peekDirection;
    }

    private void LookLKP()
    {
        context.UpdateDirection(EnemyManager.instance.LKP);
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        NextState = EnemyStateMachine.EnemyState.Fight;
        EnemyManager.instance.LKP = e.GetPos();
    }
}