using System;
using System.Collections;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    EnemyStateMachine.EnemyState NextState;
    Vector3[] worldPatrolPositions;
    bool HasCheckAroundEnded = false;

    public EnemyIdleState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey)
        : base(_context, statekey)
    {
        context = _context;
    }

    public void TransformLocalToWorld()
    {
        worldPatrolPositions = new Vector3[context.enemyAIData.PatrolPositions.Length];
        for (int i = 0; i < worldPatrolPositions.Length; i++)
            worldPatrolPositions[i] = context.parent.transform.TransformPoint(context.enemyAIData.PatrolPositions[i].Position);
    }

    public override EnemyStateMachine.EnemyState GetNextState() => NextState;

    public override IEnumerator OnStateEnter()
    {
        NextState = StateKey;
        HasCheckAroundEnded = false;
        context.events.SuspiciosEvent += OnSuspiciousEventHappen;

        context.agent.enabled = true;
        context.agent.SetDestination(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]);
        context.animationLogic.PlayWalk();

        while (!context.CheckArrived(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex],0.2f))
            yield return null;

        context.agent.enabled = false;
        HasCheckAroundEnded = true;
    }

    public override IEnumerator OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousEventHappen;
        context.rb.linearVelocity = Vector3.zero;
        yield return null;
    }

    // called once the game start in every state
    public override void Init()
    {
        TransformLocalToWorld();
    }


    // checks if enemy reaches it and sets the next patrol position or looks around
    public override void OnStateUpdate()
    {
        if (context.enemyAIData.IsStill)
            Hold();
        Patrul();
    }

    public override void OnStateFixedUpdate()
    {
        
    }

    void Patrul()
    {
        if (!HasCheckAroundEnded) return;
        MoveRigidbody();
        context.UpdateDirection(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]);
        context.animationLogic.PlayWalk();
        if (context.CheckArrived(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex], 0.2f))
        {
            // if there is a no waittime, then dont start coroutine
            HasCheckAroundEnded = false;
            float waitTime = context.enemyAIData.PatrolPositions[context.enemyAIData.CurrentPatrolPosIndex].WaitTime;
            if (waitTime > 0)
                context.coreSFM.StartCoroutine(CheckAround(waitTime));
            else
            {
                context.enemyAIData.CurrentPatrolPosIndex = (context.enemyAIData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
                HasCheckAroundEnded = true;
            }
        }
    }

    void Hold()
    {
        context.animationLogic.PlayIdlePistol();
    }

    void MoveRigidbody()
    {
        Vector3 toTarget = worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex] - context.parent.transform.position;
        toTarget.y = 0f;
        context.rb.linearVelocity = toTarget.normalized * context.enemyAIData.IdleSpeed;
    }

    // checking around when reaching one of the patrul points
    IEnumerator CheckAround(float waitTime)
    {
        context.rb.linearVelocity = Vector3.zero;
        if (waitTime > 0)
            context.animationLogic.PlayIdleLookAround();
        yield return new WaitForSeconds(waitTime);
        context.enemyAIData.CurrentPatrolPosIndex = (context.enemyAIData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
        HasCheckAroundEnded = true;
    }

    private void OnSuspiciousEventHappen(object sender, EventArgs e)
    {
        NextState = EnemyStateMachine.EnemyState.Suspicious;
    }

    
}