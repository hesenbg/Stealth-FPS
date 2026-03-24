using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MovementLogic : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private MovementData data;

    [Header("Runtime State (Read Only)")]
    public Vector3 CurrentVelocity;
    public MovementState CurrentMovementState;
    public enum MovementState { Walk, Run, Crouch, Jump, Idle }

    [Header("Control States")]
    [SerializeField] public Vector2 MoveInput;
    [HideInInspector] public bool IsSprinting;
    [SerializeField] public float SlopeDampMultipiler;

    [Header("Detection")]
    [SerializeField] float RayDistance = 1.5f;

    public bool IsGround;

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

    [SerializeField ] private float currMaxVelocity;
    private float currAcc;
    public Vector3 Direction;

    #region Unity Lifecycle
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefoultCollider = GetComponent<CapsuleCollider>();
        OnStepOnGround += HandleLanding;
    }
    #endregion

    private void Update()
    {
        Direction = UpdateDirection();
    }

    #region Procedural Logic
    Vector3 UpdateDirection()
    {
        Vector3 surfaceNormal = Vector3.up;

        if (Physics.SphereCast(DetetctionSource.position, Radius, Vector3.down, out RaycastHit hit, RayDistance, GroundMask))
        {
            surfaceNormal = hit.normal;
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
            rb.linearDamping = data.WalkSpeed*SlopeDampMultipiler;

            Vector3 rawInputDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
            return Vector3.ProjectOnPlane(rawInputDirection, surfaceNormal).normalized;
        }

        rb.useGravity = true;
        rb.linearDamping = 0;
        return new Vector3(MoveInput.x, 0, MoveInput.y);
    }
    #endregion


    #region External Executable Functions
    public void Jump()
    {
        if (IsGround || IsOnSlope)
        {
            OnStepOffGround?.Invoke(this, EventArgs.Empty);
            CurrentVelocity.y = data.JumpForce;
            CurrentMovementState = MovementState.Jump;
            rb.linearVelocity = CurrentVelocity;
        }
    }

    public void Walk()
    {
        currAcc = data.WalkAcceleration;
        currMaxVelocity = data.WalkSpeed;

        if (IsGround) CurrentMovementState = MovementState.Walk;
        ApplyHorizontalMovement();
    }

    public void Idle()
    {
        currAcc = 0f;
        currMaxVelocity = 0f;

        CurrentMovementState = MovementState.Idle;

        ApplyHorizontalMovement();
    }

    public void Run()
    {
        currAcc = data.RunAcceleration;
        currMaxVelocity = data.RunSpeed;

        if (IsGround) CurrentMovementState = MovementState.Run;
        ApplyHorizontalMovement();
    }

    public void Crouch(bool IsPressed)
    {
        if (IsPressed)
        {
            currAcc = data.CrouchAcceleration;
            currMaxVelocity = data.CrouchSpeed;
            ApplyCrouchHitbox();

            if (IsGround) CurrentMovementState = MovementState.Crouch;
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
            CurrentVelocity += Direction * currAcc * Time.deltaTime;

        Clamp();
        rb.linearVelocity = CurrentVelocity;
    }

    void Clamp()
    {

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

    private void HandleLanding(object sender, EventArgs e)
    {
        CurrentMovementState = MovementState.Idle;

        if (IsOnSlope)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
    #endregion


    #region Physics Detection
    private void OnTriggerEnter(Collider other)
    {
        if (!IsGround && other.gameObject.layer != 3)
            { return; }
            //OnStepOnGround?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        CurrentMovementState = MovementState.Jump;

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

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + CurrentVelocity.normalized * 1.5f);
    }
    #endregion
}