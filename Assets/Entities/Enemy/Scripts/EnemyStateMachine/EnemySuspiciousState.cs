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
    public override void Init()
    {
        
    }

    public override void OnStateEnter()
    {
        context.agent.enabled = true; 

        HasReached = false;
        HasInvestigated = false;
        context.agent.SetDestination(context.enemyAIData.last.Position);

        context.animationLogic.InvestigationEnd += OnInvestigationEnd;
    }

    private void OnInvestigationEnd(object sender, System.EventArgs e)
    {
        //HasInvestigated = true;
    }

    public override void OnStateExit()
    {
        context.agent.ResetPath();
        context.animationLogic.InvestigationEnd -= OnInvestigationEnd;
    }

    public override void OnStateUpdate()
    {
        if (IsReached(context.enemyAIData.last.Position) && !HasReached)
        {
            Debug.Log("reached");
            HasReached = true;
            context.agent.ResetPath();
            context.coreSFM.StartCoroutine(Investigate());
        }
    }

    public override void OnStateFixedUpdate()
    {

    }

    bool IsReached(Vector3 pos)
    {
        //Debug.Log(context.parent.transform.position +"  "+ pos);
        return Vector3.Distance(context.parent.transform.position, pos) < 1f;
    }

    IEnumerator Investigate()
    {
        yield return new WaitForSeconds(2f);
        HasInvestigated = true;
    }   
}