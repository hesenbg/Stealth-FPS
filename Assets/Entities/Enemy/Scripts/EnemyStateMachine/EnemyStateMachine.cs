using UnityEngine;
using UnityEngine.AI;
public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    [SerializeField] EnemyState current;
    public EnemyState Current => current;
    public enum EnemyState { Idle, Suspicious, Alarmed, Search, Fight }
    public EnemyStateMachineContext context {  get; private set; }

    [SerializeField] VisionCone EnemySight;

    [SerializeField] EnemyHealthManager EnemyHealthManager;

    [SerializeField] EnemyCombatLogic EnemyCombat;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] EnemyAIData data;
        
    [SerializeField] GameObject Parent;

    [SerializeField] EnemyEvents events;

    [SerializeField] EnemyAnimationLogic anim;

    private void Awake()
    {
        context = new EnemyStateMachineContext(EnemySight, EnemyHealthManager,EnemyCombat,agent,data, Parent, events, this, anim);
        InitlizeStates();
    }

    public override void UpdateStateMachine()
    {
        agent.speed = data.CurrentAwarenessState.MovementSpeed;

        current = CurrentState.StateKey;
    }

    private void InitlizeStates()
    {
        States.Add(EnemyState.Alarmed, new EnemyAlarmState(context, EnemyState.Alarmed));
        States.Add(EnemyState.Suspicious, new EnemySuspiciousState(context,EnemyState.Suspicious));
        States.Add(EnemyState.Idle, new EnemyIdleState(context, EnemyState.Idle));
        States.Add(EnemyState.Search, new EnemySearchState(context, EnemyState.Search));
        States.Add(EnemyState.Fight, new EnemyFightState(context,EnemyState.Fight));
        CurrentState = States[EnemyState.Idle];
    }
}