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
    Vector3 CachedDirectPos = Vector3.zero;
    PeekData peekdata;
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
        IsPeekAble = false;

        context.agent.ResetPath();
        yield return null;
    }
    public override IEnumerator OnStateEnter()
    {
        context.enemySight.TargetFullySeen += OnPlayerSeen;

        NextState = EnemyStateMachine.EnemyState.Alarmed;
        context.enemyAIData.IsHiding = false;
        context.agent.updateRotation = false;

        UpdateRole();

        if (CheckIfValidPeeker())
            AssignPeekParams();

        if (Alarmed == AlarmedEnemy.Peek && !IsPeekAble)
            Alarmed = AlarmedEnemy.Direct;

        CachedDirectPos = EnemyManager.instance.GetPosAroundPlayerForDirect();

        context.enemyAIData.AlarmedEnemy = Alarmed;

        context.agent.SetDestination(CachedDirectPos);
        yield return null;
    }

    public override void OnStateUpdate()
    {
        LookLKP();
        if (context.CheckArrived(EnemyManager.instance.LKP, 0.2f))
        {
            NextState = EnemyStateMachine.EnemyState.Search;
        }
    }

    void UpdateRole()
    {
        Alarmed = EnemyManager.instance.DefineAlarmedEnemy(context.parent.transform.position);
        if (Vector3.Distance(PrevLKP, EnemyManager.instance.LKP) >= context.enemyAIData.LKPchangeTolerance)
        {
        }
    }

    bool CheckIfValidPeeker()
    {
        IsPeekAble = EnemyManager.instance.FindPeekSpot(EnemyManager.instance.LKP,
            context.parent.transform.position, context.enemyAIData.CurrentAwarenessState.SightRange,
            out PeekData peekData);

        peekdata = peekData;
        return IsPeekAble;
    }

    void AssignPeekParams()
    {
        context.enemyAIData.PeekData = peekdata;
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