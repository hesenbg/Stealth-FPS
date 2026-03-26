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

    public EnemyStateMachineContext(VisionCone enemySight, HealthManager enemyHealthManager,
        ShootLogic enemyCombat, NavMeshAgent agent, EnemyAIData data, Rigidbody rb, GameObject parent)
    {
        Agent = agent;
        EnemySight = enemySight;
        EnemyHealthManager = enemyHealthManager;
        EnemyCombat = enemyCombat;
        Data = data;
        Rb = rb;
        Parent = parent;
    }
    public GameObject parent => Parent;
    public Rigidbody rb => Rb;
    public EnemyAIData enemyAIData => Data;
    public VisionCone enemySight => EnemySight;
    public HealthManager healthManager => EnemyHealthManager;
    public ShootLogic enemyCombat => EnemyCombat; 
    public NavMeshAgent agent => Agent;
}