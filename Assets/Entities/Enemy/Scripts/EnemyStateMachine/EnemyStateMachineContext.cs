using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachineContext 
{
    private Sight EnemySight;
    private HealthManager EnemyHealthManager;
    private ShootLogic EnemyCombat;
    private NavMeshAgent Agent;

    public EnemyStateMachineContext(Sight enemySight, HealthManager enemyHealthManager,
        ShootLogic enemyCombat, NavMeshAgent agent)
    {
        Agent = agent;
        EnemySight = enemySight;
        EnemyHealthManager = enemyHealthManager;
        EnemyCombat = enemyCombat;
    }

    public Sight enemySight => EnemySight;
    public HealthManager healthManager => EnemyHealthManager;
    public ShootLogic enemyCombat => EnemyCombat; 
    public NavMeshAgent agent => Agent;
}