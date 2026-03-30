using System.Collections;
using UnityEngine;

public class EnemySuspiciousState : EnemyState
{
    bool HasReached=false;
    bool HasInvestigated = false;

    public EnemySuspiciousState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        if (HasInvestigated)
            return EnemyStateMachine.EnemyState.Idle;
        return EnemyStateMachine.EnemyState.Suspicious;
    }

    // main state machine functions

    public override void OnStateEnter()
    {
        HasReached = false;
        HasInvestigated = false;
        context.agent.SetDestination(context.enemyAIData.last.Position);
    }

    public override void OnStateExit()
    {
        context.agent.ResetPath();
    }

    public override void OnStateUpdate()
    {
        if (IsReached(context.enemyAIData.last.Position) && !HasReached)
        {
            HasReached = true;
            context.agent.ResetPath();
            context.coreSFM.StartCoroutine(Investigate());
        }
    }

    bool IsReached(Vector3 pos)
    {
        return Vector3.Distance(context.parent.transform.position, pos) < 1f;
    }

    IEnumerator Investigate()
    {
        yield return new WaitForSeconds(2f);
        HasInvestigated = true;
    }
}