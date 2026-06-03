using UnityEngine;
using System.Collections;

public class EnemyAlarmState : EnemyState
{
    EnemyStateMachine.EnemyState NextState = EnemyStateMachine.EnemyState.Alarmed;
    Vector3 PlayerDirection;

    public enum AlarmedEnemy { Direct, Peek }

    enum PeekEnemy { Procces, Cover, Peek }

    public AlarmedEnemy Alarmed;

    Vector3 CoverPos;
    Vector3 PeekDirection;

    PeekEnemy peekPhase;
    Coroutine peekCoroutine;

    Vector3 PrevLKP = Vector3.zero;


    // Rusher Enemy Params

    // Peeker Enemy Params


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

        context.agent.updateRotation = true;

        context.agent.ResetPath();

        if (peekCoroutine != null)
        {
            context.coreSFM.StopCoroutine(peekCoroutine);
            peekCoroutine = null;
        }
        yield return null;
    }

    public override IEnumerator OnStateEnter()
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;

        UpdateRole();

        context.agent.updateRotation = false;

        context.enemySight.TargetFullySeen += OnPlayerSeen;

        if (Alarmed == AlarmedEnemy.Peek)
            AssignPeekParams();

        yield return null;
    }

    public override void OnStateUpdate()
    {
        LookLKP();
        ProccesAlarmedType();
    }

    void UpdateRole()
    {
        if(Vector3.Distance(PrevLKP, EnemyManager.instance.LKP) >= context.enemyAIData.LKPchangeTolerance)
        {
            Alarmed = EnemyManager.instance.DefineAlarmedEnemy(context.parent.transform.position);
        }
    }

    void AssignPeekParams()
    {
        EnemyManager.instance.FindPeekSpot(EnemyManager.instance.LKP,
            context.parent.transform.position, context.enemyAIData.CurrentAwarenessState.SightRange,
            out Vector3 peekPos, out Vector3 peekDirection);

        CoverPos = peekPos;
        PeekDirection = peekDirection;
    }

    private void ProccesAlarmedType()
    {
        PlayerDirection = (context.parent.transform.position - EnemyManager.instance.LKP).normalized;

        UpdateAnimation();

        Debug.Log(Alarmed);

        switch (Alarmed)
        {
            case AlarmedEnemy.Direct:
                UpdateDirectRusher();
                break;
            case AlarmedEnemy.Peek:
                UpdatePeeker();
                break;
        }
    }

    private void UpdateDirectRusher()
    {
        LookAround(PlayerDirection);
        context.agent.SetDestination(EnemyManager.instance.LKP);
        if (context.CheckArrived(EnemyManager.instance.LKP, 0.1f))
        {
            NextState = EnemyStateMachine.EnemyState.Search;
        }
    }

    private void UpdateAnimation()
    {
        float speed = context.agent.speed;

        if (speed < 0.1)
        {
            context.animationLogic.PlayIdlePistol();
        }
        else
        {
            context.animationLogic.PlayWalkPistol();
        }
    }

    private IEnumerator LookAround(Vector3 baseDirection)
    {
        float angle = context.enemyAIData.LookAroundAngle;
        float speed = context.enemyAIData.InterplationSpeed;

        Quaternion center = Quaternion.LookRotation(baseDirection);
        Quaternion right = Quaternion.LookRotation(Quaternion.Euler(0, angle, 0) * baseDirection);
        Quaternion left = Quaternion.LookRotation(Quaternion.Euler(0, -angle, 0) * baseDirection);

        Quaternion[] targets = { right, left, center };

        foreach (Quaternion target in targets)
        {
            while (Quaternion.Angle(context.parent.transform.rotation, target) > 0.5f)
            {
                context.parent.transform.rotation = Quaternion.RotateTowards(context.parent.transform.rotation, target, speed * Time.deltaTime);
                context.UpdateDirection (context.parent.transform.forward);
                yield return null;
            }
        }
    }

    private void UpdatePeeker()
    {
        Debug.Log("peeking");
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