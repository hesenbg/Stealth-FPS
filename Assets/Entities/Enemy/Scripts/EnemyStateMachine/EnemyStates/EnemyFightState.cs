using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAlarmState;

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

    Vector3 PlayerDir;
    Vector3 PlayerPos;
    Coroutine peekCoroutine;
    Coroutine rushCoroutine;
    Vector3[] rushPositions;
    const int RUSH_POS_COUNT = 4;
    const float RUSH_RADIUS = 0.75f;

    public override IEnumerator OnStateEnter()
    {
        context.agent.updateRotation = false;

        NextState = EnemyStateMachine.EnemyState.Fight;
        context.events.FightEvent += OnPlayerSeen;
        context.enemySight.TargetoutSight += OnTargetOutSite;
        rushPositions = new Vector3[RUSH_POS_COUNT];
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
        context.enemySight.TargetoutSight -= OnTargetOutSite;
        EnemyManager.instance.IsPlayerInSight = false;
        IsUpdated = false;
        if (peekCoroutine != null)
        {
            context.coreSFM.StopCoroutine(peekCoroutine);
            peekCoroutine = null;
        }
        if (rushCoroutine != null)
        {
            context.coreSFM.StopCoroutine(rushCoroutine);
            rushCoroutine = null;
        }
        yield return null;
    }

    private void Shoot()
    {
        if (context.enemyCombat.CanShoot())
        {
            context.enemyCombat.Shoot(EnemyManager.instance.LKP);
        }
    }   

    private void SampleRushPositions()
    {
        for (int i = 0; i < RUSH_POS_COUNT; i++)
        {
            Vector2 rand = Random.insideUnitCircle.normalized * RUSH_RADIUS;
            Vector3 candidate = PlayerPos + new Vector3(rand.x, 0f, rand.y);
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, RUSH_RADIUS, NavMesh.AllAreas))
                rushPositions[i] = hit.position;
            else
                rushPositions[i] = context.parent.transform.position;
        }
    }

    public override void OnStateUpdate()
    {
        Shoot();

        context.UpdateDirection(EnemyManager.instance.LKP);

        switch (context.enemyAIData.AlarmedEnemy)
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

    }

    private IEnumerator RushRoutine()
    {
        SampleRushPositions();
        int index = 0;
        while (true)
        {
            Vector3 target = rushPositions[index % RUSH_POS_COUNT];
            context.agent.SetDestination(target);
            yield return new WaitUntil(() => context.CheckArrived(target, 0.2f));
            float stopTimer = 0f;
            while (stopTimer < 1.5f)
            {
                stopTimer += Time.deltaTime;
                yield return null;
            }
            index++;
            if (index % RUSH_POS_COUNT == 0)
                SampleRushPositions();
        }
    }

    private void UpdatePeeker()
    {
        if (peekCoroutine == null)
            peekCoroutine = context.coreSFM.StartCoroutine(PeekRoutine());
    }

    private IEnumerator PeekRoutine()
    {
        context.enemyAIData.peekPhase = PeekEnemy.Cover;
        context.agent.SetDestination(context.enemyAIData.PeekData.CoverPos);
        yield return new WaitUntil(() => context.CheckArrived(context.enemyAIData.PeekData.CoverPos, 0.1f));
        while (true)
        {
            context.enemyAIData.peekPhase = PeekEnemy.Cover;
            context.enemyAIData.IsHiding = true;
            yield return new WaitForSeconds(context.enemyAIData.TimeBetweenPeeks);
            context.enemyAIData.peekPhase = PeekEnemy.Peek;
            context.enemyAIData.IsHiding = false;
            Vector3 peekPos = context.parent.transform.position + context.enemyAIData.PeekData.PeekDirection;
            context.agent.SetDestination(peekPos);
            yield return new WaitForSeconds(context.enemyAIData.PeekDuration);
            context.enemyAIData.peekPhase = PeekEnemy.Cover;
            context.enemyAIData.IsHiding = true;
            context.agent.SetDestination(context.enemyAIData.PeekData.CoverPos);
            yield return new WaitUntil(() => context.CheckArrived(context.enemyAIData.PeekData.CoverPos, 0.1f));
        }
    }
}