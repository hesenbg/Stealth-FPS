using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachineContext 
{
    private VisionCone EnemySight;
    private HealthManager EnemyHealthManager;
    private ShootLogic EnemyCombat;
    private NavMeshAgent Agent;
    private EnemyAIData Data;
    private Rigidbody Rb;
    private GameObject Parent;
    private EnemyEvents Events;
    private EnemyStateMachine CoreSFM;
    private EnemyAnimationLogic AnimationLogic;

    public EnemyStateMachineContext(VisionCone enemySight, HealthManager enemyHealthManager,
        ShootLogic enemyCombat, NavMeshAgent agent, EnemyAIData data, Rigidbody rb,
        GameObject parent, EnemyEvents events, EnemyStateMachine coreSFM, EnemyAnimationLogic animationLogic   )
    {
        Agent = agent;
        EnemySight = enemySight;
        EnemyHealthManager = enemyHealthManager;
        EnemyCombat = enemyCombat;
        Data = data;
        Rb = rb;
        Parent = parent;
        Events = events;
        CoreSFM = coreSFM;
        AnimationLogic = animationLogic;
    }
    public EnemyAnimationLogic animationLogic => AnimationLogic;
    public EnemyStateMachine coreSFM => CoreSFM;
    public EnemyEvents events => Events;
    public GameObject parent => Parent;
    public Rigidbody rb => Rb;
    public EnemyAIData enemyAIData => Data;
    public VisionCone enemySight => EnemySight;
    public HealthManager healthManager => EnemyHealthManager;
    public ShootLogic enemyCombat => EnemyCombat; 
    public NavMeshAgent agent => Agent;


    // helper functions for all the enemy states
    public bool CheckArrived(Vector3 pos, float Accuracy)
    {
        return Vector3.Distance(parent.transform.position, pos) < Accuracy;
    }

    // turns enemy to the given direction
    public bool UpdateDirection(Vector3 DirectionPos)
    {
        Vector3 toTarget = DirectionPos - parent.transform.position;
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