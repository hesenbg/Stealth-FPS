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

    public override void OnStateEnter()
    {
        NextState = EnemyStateMachine.EnemyState.Idle;
        HasCheckAroundEnded = true;
        context.agent.enabled = false;
        context.events.SuspiciosEvent += OnSuspiciousEventHappen;
        context.enemySight.TargetFullySeen += OnTargetFullySeen;
    }

    public override void OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousEventHappen;
        context.enemySight.TargetFullySeen -= OnTargetFullySeen;
        context.rb.linearVelocity = Vector3.zero;
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
        context.animationLogic.PlayMovementAnimation(new Vector2(context.rb.linearVelocity.x, context.rb.linearVelocity.z).magnitude);
        if (!HasCheckAroundEnded) return;
        MoveRigidbody();
        UpdateDirection();
        if (CheckArrivedRigidbody(worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex]))
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
        context.animationLogic.PlayHoldAnimation(context.enemyAIData.IsStill);
    }

    void MoveRigidbody()
    {
        Vector3 toTarget = worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex] - context.parent.transform.position;
        toTarget.y = 0f;
        context.rb.linearVelocity = toTarget.normalized * context.enemyAIData.IdleSpeed;
    }

    // rotate the enemy based on the next patrol position
    void UpdateDirection()
    {
        Vector3 toTarget = worldPatrolPositions[context.enemyAIData.CurrentPatrolPosIndex] - context.parent.transform.position;
        toTarget.y = 0f;
        if (toTarget.sqrMagnitude > 0.2f)
                context.parent.transform.rotation = Quaternion.Slerp(
                        context.parent.transform.rotation,
                        Quaternion.LookRotation(toTarget.normalized, Vector3.up),
                        Time.deltaTime * context.enemyAIData.InterplationSpeed);
    }

    bool CheckArrivedRigidbody(Vector3 worldTarget)
    {
        Vector3 flat = new Vector3(worldTarget.x, context.parent.transform.position.y, worldTarget.z);
        return Vector3.Distance(context.parent.transform.position, flat) < 0.2f;
    }

    // checking around when reaching one of the patrul points
    IEnumerator CheckAround(float waitTime)
    {
        context.rb.linearVelocity = Vector3.zero;
        if (waitTime > 0)
            context.animationLogic.PlayLookAround();
        yield return new WaitForSeconds(waitTime);
        context.animationLogic.ReturnBackLookAround();
        context.enemyAIData.CurrentPatrolPosIndex = (context.enemyAIData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
        HasCheckAroundEnded = true;
    }

    private void OnSuspiciousEventHappen(object sender, EventArgs e)
    {
        context.animationLogic.ReturnBackLookAround();
        NextState = EnemyStateMachine.EnemyState.Suspicious;
    }

    private void OnTargetFullySeen(object sender, EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Alarmed;

    private void OnTargetOutSite(object sender, EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Idle;

    private void OnTargetInSight(object sender, EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Suspicious;
}