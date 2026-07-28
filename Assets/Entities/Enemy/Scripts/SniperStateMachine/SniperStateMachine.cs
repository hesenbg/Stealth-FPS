using UnityEngine;
public class SniperStateMachine : StateManager<SniperStateMachine.SniperState>
{
    public enum SniperState {idle, Suspicious, Search, Fight }

    public SniperState current;

    public SniperContext context { get; private set; }

    [SerializeField] private GameObject controller;

    [SerializeField] private EnemyEvents events;

    [SerializeField] private VisionCone sight;

    [SerializeField] private EnemyHealthManager healthManager;

    [SerializeField] private GameObject parent;

    [SerializeField] private EnemyAIData data;

    [SerializeField] private EnemyCombatLogic enemyCombatLogic;

    private void Awake()
    {
        context = new SniperContext(controller, events, sight, healthManager, parent, data, enemyCombatLogic,this);
        InitializeStates();
    }

    public override void UpdateStateMachine()
    {
        current = CurrentState.StateKey;
    }

    void InitializeStates()
    {
        States.Add(SniperState.idle, new SniperIdleState(context,SniperState.idle));
        States.Add(SniperState.Search, new SniperSearchState(context, SniperState.Search));
        States.Add(SniperState.Suspicious, new SniperSuspiciousState(context, SniperState.Suspicious));
        States.Add(SniperState.Fight, new SniperFightState(context, SniperState.Fight));

        CurrentState = States[SniperState.idle];
    }
}