using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchState : EnemyState
{
    public EnemySearchState(EnemyStateMachineContext _context, EnemyStateMachine.EnemyState statekey) : base(_context, statekey)
    {
        context = _context;
    }

    List<Vector3> covers;
    int coverIndex;
    bool ready;
    bool done;

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        if (done) return EnemyStateMachine.EnemyState.Idle;
        return EnemyStateMachine.EnemyState.Search;
    }

    public override void Init() { }

    public override IEnumerator OnStateEnter()
    {
        ready = false;
        done = false;
        covers = EnemyManager.instance.GenerateCoverPos(4, 8, context.parent.transform.position);
        coverIndex = 0;

        covers.Insert(0,context.enemyAIData.CluePosition);

        yield return new WaitForSeconds(1.5f);
        ready = true;
        SetNextDestination();
    }

    public override IEnumerator OnStateExit()
    {
        yield return null;
    }

    public override void OnStateUpdate()
    {
        if (!ready) return;

        context.animationLogic.PlayWalkPistol();

        if (context.CheckArrived(covers[coverIndex], 0.5f))
        {
            coverIndex++;

            if (coverIndex >= covers.Count)
            {
                done = true;
                return;
            }

            SetNextDestination();
        }
    }

    void SetNextDestination()
    {
        context.agent.SetDestination(covers[coverIndex]);
    }
}