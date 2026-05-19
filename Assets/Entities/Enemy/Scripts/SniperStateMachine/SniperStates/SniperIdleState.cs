using System.Collections;
using UnityEngine;
public class SniperIdleState : SniperState
{
    public SniperIdleState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }
    Vector3[] worldPatrolPositions;
    void TransformLocalToWorld()
    {
        worldPatrolPositions = new Vector3[context.GetData.PatrolPositions.Length];
        for (int i = 0; i < worldPatrolPositions.Length; i++)
            worldPatrolPositions[i] = context.GetParent.transform.TransformPoint(context.GetData.PatrolPositions[i].Position);
    }
    public override SniperStateMachine.SniperState GetNextState()
    {
        return SniperStateMachine.SniperState.idle;
    }
    public override IEnumerator OnStateEnter()
    {
        TransformLocalToWorld();
        context.GetData.CurrentPatrolPosIndex = 0;
        context.SFM.StartCoroutine(PatrolRoutine());
        yield return null;
    }
    public override IEnumerator OnStateExit()
    {
        yield return null;
    }
    public override void OnStateUpdate()
    {
        context.GetController.UpdateRigRotation(
            worldPatrolPositions[context.GetData.CurrentPatrolPosIndex] - context.GetParent.transform.position, 0.5f);
    }
    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => context.GetController.UpdateRigRotation(
                worldPatrolPositions[context.GetData.CurrentPatrolPosIndex] - context.GetParent.transform.position, 3f));
            yield return new WaitForSeconds(context.GetData.PatrolPositions[context.GetData.CurrentPatrolPosIndex].WaitTime);
            context.GetData.CurrentPatrolPosIndex = (context.GetData.CurrentPatrolPosIndex + 1) % worldPatrolPositions.Length;
        }
    }
}