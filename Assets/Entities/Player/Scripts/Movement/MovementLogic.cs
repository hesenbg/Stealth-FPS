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
    [SerializeField] LayerMask GroundMask;
    [SerializeField] float Radius;
    private Transform Ground;

    private float currMaxVelocity;
    private float currAcc;
    public Vector3 Direction;

    #region Unity Lifecycle
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefoultCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        Direction = UpdateDirection();
    }
    #endregion


    #region Procedural Logic

    Vector3 UpdateDirection()
    {
        Vector3 surfaceNormal = Vector3.up;

        if (Physics.SphereCast(DetetctionSource.position, Radius, Vector3.down, out RaycastHit hit, RayDistance, GroundMask))
        {
            surfaceNormal = hit.normal;
            Ground = hit.transform;
            IsGround = true;
        }
        else
        {
            IsGround = false;
        }

        float slopeAngle = Vector3.Angle(Vector3.up, surfaceNormal);
        IsOnSlope = slopeAngle > 5f && slopeAngle < 45f;

        if (IsOnSlope && IsGround)
        {
            rb.useGravity = false;
            rb.linearDamping = data.WalkSpeed;

            Vector3 rawInputDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
            return Vector3.ProjectOnPlane(rawInputDirection, surfaceNormal).normalized;
        }

        rb.linearDamping = 0;
        rb.useGravity = true;
        return new Vector3(MoveInput.x, 0, MoveInput.y);
    }

    #endregion

    #region External Executable Functions
    // each function adds effect of its own then applies the direction vector as movement(regardless of the value of vector)
    public void Jump()
    {
        if (IsGround || IsOnSlope)
        {
            OnStepOffGround?.Invoke(this, EventArgs.Empty);
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
        DefoultCollider.enabled = false;
        CrouchCollider.enabled = true;  
    }

    void ReverseCrouchHitbox()
    {
        DefoultCollider.enabled = true;
        CrouchCollider.enabled = false;
    }
    #endregion

    #region Physics Detection
    // 13 ground layer
    private void OnTriggerEnter(Collider other)
    {
        Ground = other.gameObject.transform;
        if(!IsGround)
            OnStepOnGround?.Invoke(this, EventArgs.Empty);
    }
    private void OnTriggerStay(Collider other) => IsGround = true;
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
    }

    private void OnDrawGizmos()
    {
        if (DetetctionSource == null) return;

        Gizmos.color = IsGround ? Color.green : Color.red;

        Vector3 start = DetetctionSource.position;
        Vector3 end = start + Vector3.down * RayDistance;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, Radius);
        Gizmos.DrawWireSphere(end, Radius);

        if (IsGround)
        {
            Gizmos.color = Color.cyan;
            Vector3 rawInputDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
            Vector3 moveDir = UpdateDirection();

            Gizmos.DrawRay(transform.position, rawInputDirection * 2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, moveDir * 2f);
        }

        Gizmos.color = Color.red;  // velocity visual vector
        Gizmos.DrawLine(transform.position, transform.position + CurrentVelocity.normalized * 1.5f);
    }
    #endregion
}