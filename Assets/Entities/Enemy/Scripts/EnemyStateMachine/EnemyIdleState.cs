using System;
using UnityEngine;
public class EnemyIdleState : EnemyState
{
    EnemyStateMachine.EnemyState NextState;
    public EnemyIdleState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey)
        : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState() => NextState;
    public override void OnStateEnter()
    {
        NextState = EnemyStateMachine.EnemyState.Idle;
        context.events.SuspiciosEvent += OnSuspiciousEventHappen;
    }

    public override void OnStateExit()
    {
        context.events.SuspiciosEvent -= OnSuspiciousEventHappen;
        context.rb.linearVelocity = Vector3.zero;
    }

    public override void OnStateUpdate()
    {
        MoveRigidbody();
        UpdateDirection();
        if (CheckArrivedRigidbody(context.enemyAIData.PatrolPositions[context.enemyAIData.CurrentPatrolPos]))
            context.enemyAIData.CurrentPatrolPos = (context.enemyAIData.CurrentPatrolPos + 1) % context.enemyAIData.PatrolPositions.Length;
    }

    void MoveRigidbody()
    {
        Vector3 toTarget = context.enemyAIData.PatrolPositions[context.enemyAIData.CurrentPatrolPos] - context.parent.transform.position;
        toTarget.y = 0f;
        context.rb.linearVelocity = toTarget.normalized * context.enemyAIData.IdleSpeed;
    }
        
    void UpdateDirection()
    {
        Vector3 toTarget = context.enemyAIData.PatrolPositions[context.enemyAIData.CurrentPatrolPos] - context.parent.transform.position;
        toTarget.y = 0f;
        if (toTarget.sqrMagnitude > 0.001f)
            context.parent.transform.rotation = Quaternion.Slerp(
                context.parent.transform.rotation,
                Quaternion.LookRotation(toTarget.normalized, Vector3.up),
                Time.deltaTime * context.enemyAIData.InterplationSpeed);
    }

    bool CheckArrivedRigidbody(Vector3 target)
    {
        Vector3 flat = new Vector3(target.x, context.parent.transform.position.y, target.z);
        return Vector3.Distance(context.parent.transform.position, flat) < 0.2f;
    }

    private void OnSuspiciousEventHappen(object sender, EventArgs e){
        Debug.Log("fired");
        NextState = EnemyStateMachine.EnemyState.Suspicious;
    }
    private void OnTargetFullySeen(object sender, EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    private void OnTargetOutSite(object sender, EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Idle;
    private void OnTargetInSight(object sender, EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Suspicious;
}