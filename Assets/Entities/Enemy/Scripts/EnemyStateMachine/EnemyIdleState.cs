using UnityEngine;
public class EnemyIdleState : EnemyState
{
    int currentPathIndex = 0;
    bool usingRigidbody = false;

    EnemyStateMachine.EnemyState NextState;

    public EnemyIdleState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey)
        : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return NextState;
    }

    public override void OnStateEnter()
    {
        NextState = EnemyStateMachine.EnemyState.Idle;

        usingRigidbody = false;
        currentPathIndex = 0;
        SetAgentDestination(context.enemyAIData.PatrolPositions[currentPathIndex]);
        //context.enemySight.TargetSuspiciousSight += OnTargetInSight;
        //context.enemySight.TargetoutSight += OnTargetOutSite;
        //context.enemySight.TargetFullySeen += OnTargetFullySeen;
    }

    

    private void OnTargetFullySeen(object sender, System.EventArgs e)
    {
        NextState = EnemyStateMachine.EnemyState.Alarmed;
    }

    private void OnTargetOutSite(object sender, System.EventArgs e)
    {
        NextState = EnemyStateMachine.EnemyState.Idle;
    }

    private void OnTargetInSight(object sender, System.EventArgs e)
    {
        NextState = EnemyStateMachine.EnemyState.Suspicious;
    }

    public override void OnStateExit()
    {
        SwitchToAgent(); 
        context.agent.ResetPath();
    }

    public override void OnStateUpdate()
    {
        Debug.Log("idle state");


        if (!usingRigidbody)
        {
            if (!context.agent.pathPending && context.agent.remainingDistance < 0.1f)
            {
                currentPathIndex = (currentPathIndex + 1) % context.enemyAIData.PatrolPositions.Length;
                SwitchToRigidbody();
            }
        }
        else
        {
            MoveRigidbody();

            if (CheckArrivedRigidbody(context.enemyAIData.PatrolPositions[currentPathIndex]))
            {
                SwitchToAgent();
                SetAgentDestination(context.enemyAIData.PatrolPositions[currentPathIndex]);
            }
        }
    }

    void SetAgentDestination(Vector3 target)
    {
        context.agent.enabled = true;
        context.agent.SetDestination(target);
        usingRigidbody = false;
    }

    void SwitchToRigidbody()
    {
        context.agent.ResetPath();
        context.agent.enabled = false; 
        context.rb.linearVelocity = Vector3.zero;
        usingRigidbody = true;
    }

    void SwitchToAgent()
    {
        context.rb.linearVelocity = Vector3.zero; 
        context.agent.enabled = true;
        usingRigidbody = false;
    }

    void MoveRigidbody()
    {
        Vector3 toTarget = context.enemyAIData.PatrolPositions[currentPathIndex] - context.parent.transform.position;
        toTarget.y = 0f;
        context.rb.linearVelocity = toTarget.normalized * context.enemyAIData.IdleSpeed;
    }

    bool CheckArrivedRigidbody(Vector3 target)
    {
        Vector3 flat = new Vector3(target.x, context.parent.transform.position.y, target.z);
        return Vector3.Distance(context.parent.transform.position, flat) < 0.2f;
    }
}