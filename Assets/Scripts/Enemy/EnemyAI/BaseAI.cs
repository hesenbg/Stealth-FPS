using UnityEngine;
using UnityEngine.AI;

public class BaseAI : MonoBehaviour
{
    // components
    [HideInInspector] public Rigidbody rb;
    Wander WanderState;
    Suspicious SuspiciousState;
    Alarm AlarmState;
    [HideInInspector] public GuardSight Sight;
    [HideInInspector] public NavMeshAgent Agent;

    // enums && states
    public enum GuardState { Wander, Alarmed, Suspicious }
    public enum AlarmStates { Fighting, Positioning, Peeking, Null }

    public enum EnemyType { defender, rusher}

    public EnemyType OurEnemyType;

    public GuardState CurrentState;
    public GuardState LastState;

    // variables
    [HideInInspector] public Vector3 PlayerSpotPosition;
    public float DetectionRange;
    public bool IsEnemyDistracted;
    public Vector3 TriggerPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        WanderState = GetComponent<Wander>();
        Sight = GetComponent<GuardSight>();
        Agent = GetComponent<NavMeshAgent>();
        SuspiciousState = GetComponent<Suspicious>();

        // defoult state for start
        CurrentState = GuardState.Wander;
    }

    private void FixedUpdate()
    {
        UpdateStates();
        UpdateLastState();
    }

    void UpdateStates()
    {
        // update current state (alarm and sus states can be switchable anytime except the wander state)
        if (Sight.TargetOnSight)
        {
            CurrentState = GuardState.Alarmed;
        }
        if (IsEnemyDistracted)
        {
            CurrentState = GuardState.Suspicious;
        }
        else
        {
            CurrentState = LastState;
        }


        // execute current state
        switch (CurrentState)
        {
            case GuardState.Wander:
                WanderState.UpdateWander();
                Agent.enabled = false;
                break;
            case GuardState.Alarmed:
                AlarmState.UpdateAlarm();
                Agent.enabled = true;
                break;
            case GuardState.Suspicious:
                SuspiciousState.UpdateSuspicious();
                Agent.enabled = true;
                break;
        }
    }

    void UpdateLastState()
    {
        if (CurrentState == GuardState.Wander || CurrentState == GuardState.Alarmed)
        {
            LastState = CurrentState;
        }
    }

    public bool Tracktarget(Vector3 position) // guide pathfinder to reach the given destination and return weather it reached or not
    {
        Agent.SetDestination(position);

        if (IsOnTarget(position))
        {
            return true;
        }
        return false;
    }

    bool IsOnTarget(Vector3 destination) // checks if enemy's position mathc with the given position
    {
        Vector3 flatEnemyPos = new(transform.position.x, 0, transform.position.z);
        Vector3 flatDestination = new(destination.x, 0, destination.z);

        return (flatEnemyPos - flatDestination).sqrMagnitude < 1f;
    }
}
