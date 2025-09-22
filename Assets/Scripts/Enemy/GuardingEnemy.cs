using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

/// Switches between states of enemy gameobject and runs the functions according to the current state.
public class GuardingEnemy : MonoBehaviour
{
    // -------------------- Serialized Fields --------------------
    [Header("Movement & Patrol")]
    [SerializeField] Vector3[] Tracks;
    [SerializeField] float Speed;
    [SerializeField] float RotateSpeedOnTrack;

    [Header("State Control")]
    [SerializeField] float StateSwitchDelay;
    [SerializeField] float AlarmStateSwitchDelay;
    [SerializeField] public float DetectionRange;
    [SerializeField] float InvestigationTime;

    [Header("Barricade Search")]
    [SerializeField] Vector3 HalfExtend;

    [Header("References")]
    [SerializeField] PlayerMovement Player;
    [SerializeField] GameObject LeanCenter;

    // -------------------- Runtime Components --------------------
    [HideInInspector] public NavMeshAgent EnemyNavMesh;
    Rigidbody rb;
    GuardSight Sight;
    GuardWeapon Weapon;

    // -------------------- Runtime State --------------------
    Vector3 Velocity;
    Vector3 CurrentTrack;
    Vector3 Origin;
    Vector3 TriggerOriginalPosition;
    public Vector3 TriggerPosition;
    Vector3 PeekDistance;

    [HideInInspector] public Vector3 PlayerSpotPosition;

    [SerializeField] int CurrentTrackIndex;
    [SerializeField] int MaxTrackIndex;
    [SerializeField] bool HasTrack = false;
    [SerializeField] float PeekOffset;
    [SerializeField] float PeekSpeed;

    bool IsInvestigated = false;
    float alarmStateSwitchTimer;

    public bool IsEnemyDistracted = false;
    [SerializeField] bool HasTakenPosition = false;
    public bool IsDead = false;

    Collider[] AroundBarricades;
    float DistanceFromBarrier;
    BarricadePositions ClosestBarrier;

    AlarmStates lastAlarmStateBeforeDistraction = AlarmStates.Positioning;
    Vector3 lastAlarmPosition = Vector3.zero;
    BarricadePositions lastAlarmBarrier = null;
    BarricadeSide lastAlarmBarricadeSide = BarricadeSide.Left;

    bool IsPeeking;

    // Raised for access across functions
    Vector3 sideDir;
    [SerializeField] float PeekDelay = 2f; // Time to wait after peeking before returning
    Vector3 peekTargetPosition;

    [SerializeField] float PeekInterval = 5f; // Every 5 seconds
    float peekTimer;

    // choose direction
    [SerializeField] float LeanDegree;
    float PeekDelayValue;
    [SerializeField] float PeekLeanAngle;
    [SerializeField] float LeanDuration;
    [SerializeField] float ReturnDuration;

    // -------------------- Enums --------------------
    public enum GuardState { Wander, Alarmed, Triggered }
    public enum AlarmStates { Fighting, Positioning, Peeking }
    public enum BarricadeSide { Left, Right }

    public BarricadeSide EnemyBarricadeSide;
    public GuardState currentState;
    public AlarmStates CurrentAlarmState;

    // -------------------- Unity Lifecycle --------------------
    private void Start()
    {
        // components
        rb = GetComponent<Rigidbody>();
        Sight = GetComponent<GuardSight>();
        EnemyNavMesh = GetComponent<NavMeshAgent>();
        Weapon = GetComponent<GuardWeapon>();
        //LeanCenter= gameObj

        // Initialize starting values and components
        CurrentTrackIndex = 0;
        MaxTrackIndex = Tracks.Length;
        currentState = GuardState.Wander;
        EnemyNavMesh.updateRotation = false;
        ClosestBarrier = null;
    }

    private void FixedUpdate()
    {
        if (!IsDead)
        {
            // Update flat origin position for movement calculations
            Origin = new Vector3(transform.position.x, 0, transform.position.z);
            UpdateStates();
        }

        //Debug.Log(CurrentAlarmState);
    }

    // -------------------- State Machine --------------------
    private void UpdateStates()
    {
        // --- TRANSITIONS BASED ON SIGHT & SOUND ---

        if (Sight.TargetOnSight)
        {
            // SEE PLAYER  Go to ALARMED from ANY state
            if (currentState != GuardState.Alarmed)
            {
                EnterAlarmState();
            }
        }
        else if (IsEnemyDistracted)
        {
            // HEAR SOUND  Go to TRIGGERED from ANY state
            if (currentState != GuardState.Triggered)
            {
                EnterTriggeredState();
            }
        }
        else if (currentState == GuardState.Triggered && IsInvestigated)
        {
            // INVESTIGATION DONE  Return to previous state
            ExitTriggeredState();
        }

        // --- EXECUTE BEHAVIOR FOR CURRENT STATE ---

        switch (currentState)
        {
            case GuardState.Wander:
                EnemyNavMesh.enabled = false;
                Wander();
                break;

            case GuardState.Alarmed:
                EnemyNavMesh.enabled = true;
                Alarm();
                break;

            case GuardState.Triggered:
                EnemyNavMesh.enabled = true;
                TriggerEnemy();
                break;
        }
    }

    // exit enter state functions
    void EnterAlarmState()
    {
        currentState = GuardState.Alarmed;

        // If coming from Triggered, try to resume previous alarm position
        if (lastAlarmBarrier != null && Vector3.Distance(transform.position, lastAlarmPosition) < 10f)
        {
            ClosestBarrier = lastAlarmBarrier;
            TriggerPosition = lastAlarmPosition;
            EnemyBarricadeSide = lastAlarmBarricadeSide;
            HasTakenPosition = false; // Force re-check position (in case player moved barricade or it’s blocked)
        }
        else
        {
            // Fresh alarm — find new barricade
            ClosestBarrier = null;
            HasTakenPosition = false;
        }

        // Reset peeking
        IsPeeking = false;
        peekTimer = 0;
        CurrentAlarmState = AlarmStates.Positioning; // Start by finding cover
    }

    void EnterTriggeredState()
    {
        // Save alarm context if currently alarmed
        if (currentState == GuardState.Alarmed)
        {
            lastAlarmStateBeforeDistraction = CurrentAlarmState;
            lastAlarmPosition = TriggerPosition;
            lastAlarmBarrier = ClosestBarrier;
            lastAlarmBarricadeSide = EnemyBarricadeSide;
        }

        //  Save the state we were in BEFORE becoming Triggered
        LastGuardState = currentState;

        currentState = GuardState.Triggered;
        TriggerOriginalPosition = transform.position;
        IsInvestigated = false;
        EnemyNavMesh.enabled = true;
    }

    void ExitTriggeredState()
    {
        IsEnemyDistracted = false;
        IsInvestigated = false;

        // If player was seen during investigation  go back to Alarmed
        if (Sight.TargetOnSight)
        {
            EnterAlarmState();
        }
        else
        {
            //  Return to the state we were in BEFORE being distracted
            currentState = LastGuardState;
            EnemyNavMesh.enabled = false;
        }
    }

    // -------------------- Wander Behavior --------------------
    void Wander()  // defoult state that goes through certain positions(track - position)
    {
        CheckHasTrack();
        UpdateRotation(Tracks[CurrentTrackIndex]);
    }
    void Alarm()
    {
        // --- DECIDE SUB-STATE ---

        if (Sight.TargetOnSight)
        {
            TrySwitchAlarmState(AlarmStates.Fighting);
        }
        else if (!HasTakenPosition && !IsPeeking)
        {
            TrySwitchAlarmState(AlarmStates.Positioning);
        }
        else
        {
            //TrySwitchAlarmState(AlarmStates.Peeking);
        }

        // --- EXECUTE SUB-STATE ---

        switch (CurrentAlarmState)
        {
            case AlarmStates.Fighting:
                Weapon.IsShooting = true;
                UpdateRotation(PlayerSpotPosition);
                break;

            case AlarmStates.Positioning:
                Weapon.IsShooting = false;
                if (TakePosition())
                {
                    HasTakenPosition = true;
                    // Schedule first peek
                    peekTimer = Time.time + PeekInterval;
                }
                break;

            case AlarmStates.Peeking:
                StartCoroutine(PeekCycle());
                // Schedule next peek
                peekTimer = Time.time + PeekInterval;
                // After peek, go back to Positioning until next peek time
                break;
        }
    }

    GuardState LastGuardState;
    void TriggerEnemy() // checks position and goes back 
    {
        UpdateRotation(TriggerPosition);

        if (Tracktarget(TriggerPosition))
        {
            StartCoroutine(Investigate());
            if (IsInvestigated)
            {
                currentState = LastGuardState;
                IsEnemyDistracted = false;
                return;
            }
        }
    }

    void TrySwitchAlarmState(AlarmStates desiredState) // delay between state switching
    {
        if (CurrentAlarmState == desiredState) return;

        if (Time.time >= alarmStateSwitchTimer)
        {
            CurrentAlarmState = desiredState;
            alarmStateSwitchTimer = Time.time + AlarmStateSwitchDelay;
        }
    }

    bool TakePosition()  // sreach up a barricade, 
    {
        DistanceFromBarrier = Mathf.Infinity;

        if (!HasTakenPosition && !IsPeeking)
        {
            ClosestBarrier = FindClosest();
            if (ClosestBarrier == null)
            {
                return false;
            }
            TriggerPosition = ClosestBarrier.FindClosestPosition(transform.position);
            EnemyBarricadeSide = (ClosestBarrier.IsRight) ? BarricadeSide.Right : BarricadeSide.Left;
            CurrentAlarmState = AlarmStates.Fighting;
            if (Tracktarget(TriggerPosition))
            {
                Debug.Log("reached the position");
                HasTakenPosition = true;
                //EnemyNavMesh.enabled = false;
                return true;
            }
            return false ;
        }
        else
        {
            if (PeekDelayValue < PeekDelay)
            {
                PeekDelayValue += Time.deltaTime;
            }
            else
            {
                PeekDelayValue = 0;
                CurrentAlarmState = AlarmStates.Peeking;
            }
        }


        UpdateRotation(PlayerSpotPosition);
        if (EnemyNavMesh.remainingDistance <= EnemyNavMesh.stoppingDistance)
            Debug.Log("Already at destination");


        return false;
    }

    IEnumerator PeekCycle()
    {
        if (LeanCenter == null) yield break;
        Debug.Log("peeking");
        if (Sight.CanSeePlayer())
        {
            TrySwitchAlarmState(AlarmStates.Fighting);
        }

        IsPeeking = true;

        Quaternion originalRotation = LeanCenter.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, EnemyBarricadeSide == BarricadeSide.Left ? PeekLeanAngle : -PeekLeanAngle);

        // Lean out
        float elapsed = 0;
        while (elapsed < LeanDuration)
        {
            elapsed += Time.deltaTime;
            LeanCenter.transform.localRotation = Quaternion.Slerp(originalRotation, targetRotation, elapsed / LeanDuration);
            yield return null;
        }


        // Return to cover
        elapsed = 0;
        while (elapsed < ReturnDuration)
        {
            elapsed += Time.deltaTime;
            LeanCenter.transform.localRotation = Quaternion.Slerp(LeanCenter.transform.localRotation, originalRotation, elapsed / ReturnDuration);
            yield return null;
        }

        LeanCenter.transform.localRotation = originalRotation;
        IsPeeking = false;
        CurrentAlarmState = AlarmStates.Positioning;
    }
    // -------------------- Triggered Behavior -------------------- 
    BarricadePositions FindClosest()
    {
        ClosestBarrier = null;

        AroundBarricades = Physics.OverlapBox(
        transform.position,
        HalfExtend,
        Quaternion.identity,
        LayerMask.GetMask("Barricade")
        );

        foreach (Collider c in AroundBarricades)
        {
            float dist = Vector3.Distance(c.transform.position, transform.position);
            if (dist < DistanceFromBarrier)
            {
                DistanceFromBarrier = dist;
                ClosestBarrier = c.GetComponent<BarricadePositions>();
            }
        }
        if(ClosestBarrier == null)
        {
            HasTakenPosition = false;
        }
        else
        {
            Debug.Log("barricade found");
            HasTakenPosition = true;
        }

        return ClosestBarrier;
    }

    IEnumerator Investigate()
    {
        EnemyNavMesh.speed = 0;
        yield return new WaitForSeconds(InvestigationTime);
        IsInvestigated = true;
        TriggerPosition = TriggerOriginalPosition;
        EnemyNavMesh.speed = 3;
    }

    // -------------------- Utility --------------------
    public bool Tracktarget(Vector3 position)
    {
        EnemyNavMesh.SetDestination(position);

        if (IsOnTarget(position))
        {
            return true;
        }
        return false;
    }


    bool IsOnTarget(Vector3 destination)
    {
        Vector3 flatEnemyPos = new(transform.position.x, 0, transform.position.z);
        Vector3 flatDestination = new(destination.x, 0, destination.z);

        return (flatEnemyPos - flatDestination).sqrMagnitude < 1f;
    }

    bool IsPlayerInNoticeableArea()
    {
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPos = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);

        float sqrDistance = (enemyPos - playerPos).sqrMagnitude;
        return sqrDistance <= DetectionRange * DetectionRange;
    }

    void UpdateRotation(Vector3 target)
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
                Time.deltaTime * RotateSpeedOnTrack
            );
        }
    }

    void UpdateRotationVelocity()
    {
        Vector3 velocity = rb.linearVelocity;
        if (velocity.x != 0 || velocity.z != 0)
        {
            Vector3 direction = new Vector3(velocity.x, 0, velocity.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    void GetTrackIndex()  // gets the next index from list that will be the position
    {
        if (CurrentTrackIndex < MaxTrackIndex - 1)
            CurrentTrackIndex++;
        else
            CurrentTrackIndex = 0;

        CurrentTrack = Tracks[CurrentTrackIndex];
        HasTrack = true;
    }

    void CheckHasTrack()  // forward enemy to the next track and checks it of reached
    {
        if (!HasTrack)
            GetTrackIndex();
        else
            HasTrack = TrackPositionDirect(CurrentTrack);
    }

    bool TrackPositionDirect(Vector3 targetPosition) // gives velocity based on given position
    {
        Vector3 direction = new Vector3(targetPosition.x - Origin.x, 0, targetPosition.z - Origin.z);
        Velocity = direction.normalized;
        rb.linearVelocity = new Vector3(Velocity.x * Speed, 0, Velocity.z * Speed);

        return direction.magnitude >= 0.2f;
    }

    // -------------------- Gizmos --------------------
    private void OnDrawGizmos()
    {
        if (Tracks == null || Tracks.Length == 0) return;

        Gizmos.color = Color.green;

        foreach (var track in Tracks)
        {
            Vector3 pos = new Vector3(track.x, transform.position.y, track.z);
            Gizmos.DrawSphere(pos, 0.3f);
        }

        for (int i = 0; i < Tracks.Length; i++)
        {
            Vector3 current = new Vector3(Tracks[i].x, transform.position.y, Tracks[i].z);
            Vector3 next = new Vector3(Tracks[(i + 1) % Tracks.Length].x, transform.position.y, Tracks[(i + 1) % Tracks.Length].z);
            Gizmos.DrawLine(current, next);
        }

        Gizmos.DrawWireCube(transform.position, HalfExtend * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(CurrentTrack.x, transform.position.y, CurrentTrack.z), 0.4f);

        Gizmos.color = Color.black;
        if (HasTakenPosition)
            Gizmos.DrawSphere(PeekDistance, 0.5f);


        Gizmos.color = Color.black;
        //Gizmos.DrawSphere(peekTargetPosition, 1);
        Gizmos.DrawSphere(TriggerPosition, 0.3f);
    }
}