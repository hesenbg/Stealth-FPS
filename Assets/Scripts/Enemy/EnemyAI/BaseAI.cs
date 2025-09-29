using UnityEngine;
using UnityEngine.AI;

public class BaseAI : MonoBehaviour
{
    // components
    [HideInInspector] public Rigidbody rb;
    Wander WanderState;
    Suspicious SuspiciousState;
    [HideInInspector] public Alarm AlarmState;
    [HideInInspector] public GuardSight Sight;
    [HideInInspector] public NavMeshAgent Agent;

    // enums && states
    public enum GuardState { Wander, Alarmed, Suspicious }
    public enum AlarmStates { Fighting, Positioning, Peeking, Null }

    public enum EnemyType { defender, rusher}
    public GuardState CurrentState;
    public GuardState LastState;

    // variables
    [HideInInspector] public Vector3 PlayerSpotPosition;
    public float DetectionRange;
    public bool IsEnemyDistracted;
    public Vector3 TriggerPosition;

    float StateSwitchDelayValue;

    private void Start()
    {
        // state classes
        WanderState = GetComponent<Wander>();
        Sight = GetComponent<GuardSight>();
        AlarmState = GetComponent<Alarm>();

        // components
        rb = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
        SuspiciousState = GetComponent<Suspicious>();

        // defoult state for start
        CurrentState = GuardState.Wander;
        StateSwitchDelayValue = 0;
    }

    private void FixedUpdate()
    {
        RunCurrentState();
        UpdateLastState();
    }

    void RunCurrentState()
    {
        UpdateCurrentState();
        ExecuteCurrentState();

    }

    void UpdateCurrentState()
    {
        if (Sight.TargetOnSight)
        {
            CurrentState = GuardState.Alarmed;
            return;
        }
        if (IsEnemyDistracted)
        {
            CurrentState = GuardState.Suspicious;
        }
        else
        {
            CurrentState = LastState;
        }
    }

    void ExecuteCurrentState()
    {
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

    public bool Tracktarget(Vector3 position, float Speed) // guide pathfinder to reach the given destination and return weather it reached or not
    {

        if (IsOnTarget(position))
        {
            return true;
        }
        else
        {
            Agent.SetDestination(position);
            Agent.speed = Speed;
            return false;        
        }
    }

    bool IsOnTarget(Vector3 destination) // checks if enemy's position mathc with the given position
    {
        Vector3 flatEnemyPos = new(transform.position.x, 0, transform.position.z);
        Vector3 flatDestination = new(destination.x, 0, destination.z);

        return (flatEnemyPos - flatDestination).sqrMagnitude < 1f;
    }
    public void UpdateRotation(Vector3 target, float Speed)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion offsetRotation = Quaternion.Euler(0, 0, 0); // meaningless but looks cool
            Quaternion finalRotation = targetRotation * offsetRotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                finalRotation,
                Time.deltaTime * Speed
            );
        }
    }

    public void SwitchState(GuardState Current, GuardState Desired,float Delay)
    {
        if (StateSwitchDelayValue < Delay)
        {
            StateSwitchDelayValue += Time.deltaTime;
        }
        else
        {
            StateSwitchDelayValue = 0;
            Current = Desired;
        }

    }
}
