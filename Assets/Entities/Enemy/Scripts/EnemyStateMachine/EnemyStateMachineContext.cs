using UnityEngine;
using UnityEngine.AI;
public class EnemyStateMachineContext 
{
    private VisionCone EnemySight;
    private HealthManager EnemyHealthManager;
    private ShootLogic EnemyCombat;
    private NavMeshAgent Agent;
    private EnemyAIData Data;
    private GameObject Parent;
    private EnemyEvents Events;
    private EnemyStateMachine CoreSFM;
    private EnemyAnimationLogic AnimationLogic;

    public EnemyStateMachineContext(VisionCone enemySight, HealthManager enemyHealthManager,
        ShootLogic enemyCombat, NavMeshAgent agent, EnemyAIData data,
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
    public HealthManager healthManager => EnemyHealthManager;
    public ShootLogic enemyCombat => EnemyCombat; 
    public NavMeshAgent agent => Agent;

    // helper functions for all the enemy states
    public bool CheckArrived(Vector3 pos, float Accuracy) // assumes that distance between enemy's pivot and ground is 1.7 (taller than lulu xd)
    {
        float RealDistance = Vector3.Distance(parent.transform.position, pos);

        float GroundDistance = Vector2.Distance(new Vector2(parent.transform.position.x, parent.transform.position.z)
            , new Vector2(pos.x,pos.z));

        return (GroundDistance < Accuracy && RealDistance < 1.85);
    }

    public void ResetVelocity()
    {
        agent.velocity = Vector3.zero;
    }


    // turns enemy to the given direction
    public bool UpdateDirection(Vector3 Direction)
    {
        Vector3 toTarget = Direction - parent.transform.position;
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