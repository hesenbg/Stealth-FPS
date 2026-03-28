using System;
using UnityEngine;
public class EnemyIdleState : EnemyState
{
    int currentPathIndex = 0;
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
        currentPathIndex = 0;

        context.events.SuspiciosEvent += OnSuspiciousEventHappen;
    }

    public override void OnStateExit()
    {
        context.rb.linearVelocity = Vector3.zero;
    }

    public override void OnStateUpdate()
    {
        MoveRigidbody();
        UpdateDirection();
        if (CheckArrivedRigidbody(context.enemyAIData.PatrolPositions[currentPathIndex]))
            currentPathIndex = (currentPathIndex + 1) % context.enemyAIData.PatrolPositions.Length;
    }

    void MoveRigidbody()
    {
        Vector3 toTarget = context.enemyAIData.PatrolPositions[currentPathIndex] - context.parent.transform.position;
        toTarget.y = 0f;
        context.rb.linearVelocity = toTarget.normalized * context.enemyAIData.IdleSpeed;
    }
        
    void UpdateDirection()
    {
        Vector3 toTarget = context.enemyAIData.PatrolPositions[currentPathIndex] - context.parent.transform.position;
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

    private void OnSuspiciousEventHappen(object sender, System.EventArgs e){
        NextState = EnemyStateMachine.EnemyState.Suspicious;
        Debug.Log("SUs event happend negga");
    }
    private void OnTargetFullySeen(object sender, System.EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    private void OnTargetOutSite(object sender, System.EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Idle;
    private void OnTargetInSight(object sender, System.EventArgs e) =>
        NextState = EnemyStateMachine.EnemyState.Suspicious;
}