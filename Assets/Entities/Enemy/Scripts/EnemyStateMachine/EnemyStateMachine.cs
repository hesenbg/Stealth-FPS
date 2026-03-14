using UnityEngine;
using UnityEngine.AI;
public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    public enum EnemyState { Idle, Suspicious, Alarmed, Search }

    public EnemyStateMachineContext context {  get; private set; }

    [SerializeField] Sight EnemySight;

    [SerializeField] HealthManager EnemyHealthManager;

    [SerializeField] ShootLogic EnemyCombat;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] EnemyAIData data;

    [SerializeField] Rigidbody rb;

    [SerializeField] GameObject Parent;

    [SerializeField] Audiotary audiotary;

    private void Awake()
    {
        context = new EnemyStateMachineContext(EnemySight, EnemyHealthManager,EnemyCombat,agent,data,rb, Parent);

        InitlizeStates();
    }

    private void InitlizeStates()
    {
        States.Add(EnemyState.Alarmed, new EnemyAlarmState(context, EnemyState.Alarmed));
        States.Add(EnemyState.Suspicious, new EnemySuspiciousState(context,EnemyState.Suspicious));
        States.Add(EnemyState.Idle, new EnemyIdleState(context, EnemyState.Idle));
        States.Add(EnemyState.Search, new EnemySearchState(context, EnemyState.Search));
        CurrentState = States[EnemyState.Idle];
    }
}