using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyStateMachineContext 
{
    private VisionCone EnemySight;
    private EnemyHealthManager EnemyHealthManager;
    private EnemyCombatLogic EnemyCombat;
    private NavMeshAgent Agent;
    private EnemyAIData Data;
    private GameObject Parent;
    private EnemyEvents Events;
    private EnemyStateMachine CoreSFM;
    private EnemyAnimationLogic AnimationLogic;

    public EnemyStateMachineContext(VisionCone enemySight, EnemyHealthManager enemyHealthManager,
        EnemyCombatLogic enemyCombat, NavMeshAgent agent, EnemyAIData data,
        GameObject parent, EnemyEvents events, EnemyStateMachine coreSFM, EnemyAnimationLogic animationLogic   )
    {
        Agent = agent;
        EnemySight = enemySight;
        EnemyHealthManager = enemyHealthManager;
        EnemyCombat = enemyCombat;
        Data = data;
        Parent = parent;
        Events = events;
        CoreSFM = coreSFM;
        AnimationLogic = animationLogic;
    }
    public EnemyAnimationLogic animationLogic => AnimationLogic;
    public EnemyStateMachine coreSFM => CoreSFM;
    public EnemyEvents events => Events;
    public GameObject parent => Parent;
    public EnemyAIData enemyAIData => Data;
    public VisionCone enemySight => EnemySight;
    public EnemyHealthManager healthManager => EnemyHealthManager;
    public EnemyCombatLogic enemyCombat => EnemyCombat; 
    public NavMeshAgent agent => Agent;

    // helper functions for all the enemy states
    public bool CheckArrived(Vector3 TargetPos, float Accuracy) // check if enemy see the destination or not
    {
        Vector3 Direction = (TargetPos - parent.transform.position).normalized;
        float Distance = Vector3.Distance(parent.transform.position, TargetPos);

        if (Distance < Accuracy) return true; 

        if (Physics.Raycast(parent.transform.position, Direction, out RaycastHit hit, Distance))
        {
            return Vector3.Distance(hit.point, TargetPos) < Accuracy; 
        }

        return false; 
    }


    public IEnumerator LookAround(Vector3 Direction, float Duration, float Angle, float Speed)
    {
        float perWait = Duration / 3f;
        Quaternion original = Quaternion.LookRotation(Direction, Vector3.up);
        Quaternion left = Quaternion.LookRotation(Quaternion.AngleAxis(-Angle, Vector3.up) * Direction, Vector3.up);
        Quaternion right = Quaternion.LookRotation(Quaternion.AngleAxis(Angle, Vector3.up) * Direction, Vector3.up);

        yield return RotateTo(original, perWait);
        yield return RotateTo(left, perWait);
        yield return RotateTo(right, perWait);
        yield return RotateTo(original, perWait);
    }

    private IEnumerator RotateTo(Quaternion target, float duration)
    {
        float elapsed = 0f;
        Quaternion start = parent.transform.rotation;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            parent.transform.rotation = Quaternion.Slerp(start, target, elapsed / duration);
            yield return null;
        }
        parent.transform.rotation = target;
    }

    public void ResetLookAround()
    {
        parent.transform.rotation = Quaternion.LookRotation(
            new Vector3(parent.transform.forward.x, 0f, parent.transform.forward.z).normalized, Vector3.up);
    }

    public void ResetVelocity()
    {
        agent.velocity = Vector3.zero;
    }


    // turns enemy to the given direction
    public bool UpdateDirection(Vector3 Position)
    {
        Vector3 toTarget = Position - parent.transform.position;
        toTarget.y = 0f;
        if (toTarget.sqrMagnitude > 0.2f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
            parent.transform.rotation = Quaternion.Slerp(
                    parent.transform.rotation,
                    targetRot,
                    Time.deltaTime * enemyAIData.InterplationSpeed);
            return Quaternion.Angle(parent.transform.rotation, targetRot) < 1f;
        }
        return true;
    }
}