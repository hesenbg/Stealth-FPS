using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
// how does it works
// it only updates the procedural logic.
// you give the direction vector as data  and inputs apply the effect
// effects are applies regardless of the direction input
public class MovementLogic : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private MovementData data;

    [Header("Runtime State (Read Only)")]
    public Vector3 CurrentVelocity;
    public MovementState CurrentMovementState;
    public enum MovementState { Walk, Run, Crouch, Jump, Idle }
    
    [Header("Control States")]
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public bool IsSprinting;

    [Header("Detection")]
    [SerializeField] float RayDistance = 1.5f;
    public bool IsGround  { get; private set; }
    [SerializeField] bool IsOnSlope = false;
    [SerializeField] Transform DetetctionSource;

    public event EventHandler OnStepOnGround;
    public event EventHandler OnStepOffGround;

    [Header("References")]
    private Rigidbody rb;
    [SerializeField] CapsuleCollider DefoultCollider;
    [SerializeField] CapsuleCollider CrouchCollider;
    private BoxCollider GroundTrigger;

    private float currMaxVelocity;
    private float currAcc;
    private float baseHeight;
    private Vector3 standGroundCheck;
    public Vector3 Direction;

    #region Unity Lifecycle
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefoultCollider = GetComponent<CapsuleCollider>();
        GroundTrigger = GetComponent<BoxCollider>();

        baseHeight = DefoultCollider.height;
        standGroundCheck = GroundTrigger.center;
    }

    private void Update()
    {
        Direction = UpdateDirection();
    }
    #endregion


    #region Procedural Logic

    Vector3 UpdateDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(DetetctionSource.position, Vector3.down, out hit, RayDistance))
        {
            Vector3 surfaceNormal = hit.normal;
            IsOnSlope = Vector3.Angle(Vector3.up, surfaceNormal) > 5f;


            if (IsOnSlope && IsGround)
            {
                rb.useGravity = false; // added to avoid falling down
                rb.linearDamping = data.WalkSpeed; // moving in slope with gravity of disables deacceleration so we add damping
                return Vector3.ProjectOnPlane(Direction, surfaceNormal).normalized;
            }
        }

        rb.linearDamping = 0;
        IsOnSlope = false;
        rb.useGravity = true;
        return Direction;
    }

    #endregion

    #region External Executable Functions
    // each function adds effect of its own then applies the direction vector as movement(regardless of the value of vector)
    public void Jump()
    {
        if (IsGround || IsOnSlope)
        {
            CurrentVelocity = rb.linearVelocity;
            CurrentVelocity.y = data.JumpForce;
            CurrentMovementState = MovementState.Jump;
            rb.linearVelocity = CurrentVelocity;
        }
    }

    public void Walk()
    {
        CurrentMovementState = MovementState.Walk;
        currAcc = data.WalkAcceleration;
        currMaxVelocity = data.WalkSpeed;

        ApplyHorizontalMovement();
    }

    public void Idle()
    {
        CurrentMovementState = MovementState.Idle;
        currAcc = 0f;
        currMaxVelocity = 0f;
        ApplyHorizontalMovement();
    }

    public void Run()
    {
        CurrentMovementState = MovementState.Run;
        currAcc = data.RunAcceleration;
        currMaxVelocity = data.RunSpeed;

        ApplyHorizontalMovement();
    }

    public void Crouch(bool IsPressed)
    {
        if (IsPressed)
        {
            CurrentMovementState = MovementState.Crouch;
            currAcc = data.CrouchAcceleration;
            currMaxVelocity = data.CrouchSpeed;
            ApplyCrouchHitbox();
            ApplyHorizontalMovement();
        }
        else
        {
            ReverseCrouchHitbox();
        }
    }
    #endregion

    #region Internal Movement Logic
    void ApplyHorizontalMovement()
    {
        CurrentVelocity = rb.linearVelocity;
        Direction = UpdateDirection();

        if (Direction.magnitude > 0)
        {
            CurrentVelocity += Direction * currAcc * Time.deltaTime;
        }
        Clamp();
        rb.linearVelocity = CurrentVelocity;
    }
    // limits the max velocity
    void Clamp()
    {
        if (CurrentMovementState == MovementState.Jump) return;

        float verticalVel = CurrentVelocity.y;
        Vector3 horizontalVel = new Vector3(CurrentVelocity.x, 0, CurrentVelocity.z);

        if (IsOnSlope)
        {
            if (CurrentVelocity.magnitude > currMaxVelocity && currMaxVelocity != 0)
                CurrentVelocity = CurrentVelocity.normalized * currMaxVelocity;
        }
        else
        {
            if (horizontalVel.magnitude > currMaxVelocity && currMaxVelocity != 0)
            {
                horizontalVel = horizontalVel.normalized * currMaxVelocity;
                CurrentVelocity = new Vector3(
                    horizontalVel.x,
                    verticalVel,
                    horizontalVel.z
                );
            }
        }
    }

    void ApplyCrouchHitbox()
    {
        DefoultCollider.height = Mathf.Lerp(
            DefoultCollider.height,
            data.CrouchHitboxHeight,
            data.CrouchLerpSpeed * Time.deltaTime
        );

        GroundTrigger.center = Vector3.Lerp(
            GroundTrigger.center,
            data.CrouchGroundCheck,
            data.CrouchLerpSpeed * Time.deltaTime
        );

        DetetctionSource.localPosition = new Vector3(0f,-1f,0f);
    }

    void ReverseCrouchHitbox()
    {
        DefoultCollider.height = Mathf.Lerp(
            DefoultCollider.height,
            baseHeight,
            data.CrouchLerpSpeed * Time.deltaTime
        );

        GroundTrigger.center = Vector3.Lerp(
            GroundTrigger.center,
            standGroundCheck,
            data.CrouchLerpSpeed * Time.deltaTime
        );

        DetetctionSource.localPosition = new Vector3(0f, -1.5f, 0f);
    }
    #endregion

    #region Physics Detection
    private void OnTriggerEnter(Collider other)
    {
        IsGround = true;
        OnStepOnGround?.Invoke(this,EventArgs.Empty);
    }
    private void OnTriggerStay(Collider other) => IsGround = true;
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
        OnStepOffGround?.Invoke(this,EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // ground detection vector
        Gizmos.DrawLine(DetetctionSource.position, DetetctionSource.position + Vector3.down * RayDistance);

        Gizmos.color = Color.red;  // velocity visual vector
        Gizmos.DrawLine(transform.position, transform.position + CurrentVelocity.normalized * 1.5f);
    }
    #endregion
}