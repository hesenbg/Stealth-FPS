using UnityEngine;
using UnityEngine.AI;

public class BaseAI : MonoBehaviour
{
    // components
    [HideInInspector] public Rigidbody rb;
    Wonder WanderState;
    Suspicious SuspiciousState;
    [HideInInspector] public Alarm AlarmState;
    [HideInInspector] public GuardSight Sight;
    [HideInInspector] public NavMeshAgent Agent;
    [HideInInspector] public EnemyAnimationLogic AnimationLogic;

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

    [SerializeField] float SuspiciousStateDelay;
    [SerializeField] float AlarmStateDelay;
    private void Start()
    {
        // state classes
        WanderState = GetComponent<Wonder>();
        Sight = GetComponent<GuardSight>();
        AlarmState = GetComponent<Alarm>();

        // components
        rb = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
        SuspiciousState = GetComponent<Suspicious>();
        AnimationLogic = GameObject.Find("EnemyMesh").GetComponent<EnemyAnimationLogic>();

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
            //CurrentState = GuardState.Alarmed;
            CurrentState= SwitchStateDelay(CurrentState, GuardState.Alarmed, AlarmStateDelay);
            return;
        }
        if (IsEnemyDistracted)
        {
            CurrentState = SwitchStateDelay(CurrentState,GuardState.Suspicious, SuspiciousStateDelay);
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
                Agent.enabled = false;
                WanderState.UpdateWander();
                break;
            case GuardState.Alarmed:
                Agent.enabled = true;
                AlarmState.UpdateAlarm();
                break;
            case GuardState.Suspicious:
                Agent.enabled = true;
                SuspiciousState.UpdateSuspicious();
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

    public GuardState SwitchStateDelay(GuardState Current, GuardState Desired,float Delay)
    {
        if (StateSwitchDelayValue < Delay)
        {
            StateSwitchDelayValue += Time.deltaTime;
            return Current;
        }
        else
        {
            StateSwitchDelayValue = 0;
            return Desired;
        }
    }


    void SwitchToAlarm()
    {

    }
}
