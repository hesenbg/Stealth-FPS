using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private PlayerMovementData data;

    [Header("Runtime State (Read Only)")]
    public Vector3 CurrentVelocity;
    public MovementState CurrentMovementState;
    public enum MovementState { Walk, Run, Crouch, Jump, Idle }

    [Header("Control States")]
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public bool IsSprinting;

    [Header("Detection")]
    [SerializeField] float RayDistance = 1.5f;
    [SerializeField] bool IsGround = false;
    [SerializeField] bool IsOnSlope = false;

    [Header("Obstacle Avoidance")]
    [SerializeField] Transform LowerPos;
    [SerializeField] Transform UpperPos;
    [SerializeField] Vector3 HalfExtend;
    [SerializeField] Vector3 RigidbodyUp;

    [Header("References")]
    private Rigidbody rb;
    private CapsuleCollider PlayerHitbox;
    private BoxCollider GroundTrigger;

    private float currMaxVelocity;
    private float currAcc;
    private float baseHeight;
    private Vector3 standGroundCheck;

    #region Unity Lifecycle
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerHitbox = GetComponent<CapsuleCollider>();
        GroundTrigger = GetComponent<BoxCollider>();

        baseHeight = PlayerHitbox.height;
        standGroundCheck = GroundTrigger.center;
    }

    private void Update()
    {
        Vector3 movementDirection = UpdateDirection();
        CheckObstacle();
        //HandleMovementExecution(movementDirection);
        ApplyVelocity();
    }
    #endregion

    #region Procedural Logic

    Vector3 UpdateDirection()
    {
        RaycastHit hit;
        Vector3 calculatedDir = transform.forward * MoveInput.y + transform.right * MoveInput.x;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance))
        {
            Vector3 surfaceNormal = hit.normal;
            Debug.Log(hit.normal);
            IsOnSlope = Vector3.Angle(Vector3.up, surfaceNormal) > 1f;

            if (IsOnSlope && IsGround)
            {
                rb.useGravity = false;
                rb.linearDamping = 4f;
                return Vector3.ProjectOnPlane(calculatedDir, surfaceNormal).normalized;
            }
        }

        rb.useGravity = true;
        rb.linearDamping = 0f;
        return calculatedDir.normalized;
    }



    void CheckObstacle()
    {
        if (Physics.OverlapBox(LowerPos.position, HalfExtend, transform.rotation).Length >= 1)
        {
            if (Physics.OverlapBox(UpperPos.position, HalfExtend, transform.rotation).Length == 0)
            {
                rb.position = Vector3.Lerp(rb.position, rb.position + RigidbodyUp, Time.deltaTime * data.CrouchLerpSpeed);
            }
        }
    }

    void ApplyVelocity() => rb.linearVelocity = CurrentVelocity;
    #endregion

    #region External Executable Functions
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

    public void Walk(Vector3 direction)
    {
        CurrentMovementState = MovementState.Walk;
        currAcc = data.WalkAcceleration;
        currMaxVelocity = data.WalkSpeed;

        ApplyHorizontalMovement(direction);
        Clamp();
    }

    public void Run(Vector3 direction)
    {
        CurrentMovementState = MovementState.Run;
        currAcc = data.RunAcceleration;
        currMaxVelocity = data.RunSpeed;

        ApplyHorizontalMovement(direction);
        Clamp();
    }

    public void Crouch(Vector3 direction, bool IsPressed)
    {
        if (IsPressed)
        {
            CurrentMovementState = MovementState.Crouch;
            currAcc = data.CrouchAcceleration;
            currMaxVelocity = data.CrouchSpeed;
            ApplyCrouchHitbox();
            ApplyHorizontalMovement(direction);
        }
        else
        {
            ReverseCrouchHitbox();
        }
        Clamp();
    }
    #endregion

    #region Internal Movement Logic
    void ApplyHorizontalMovement(Vector3 direction)
    {
        Vector3 rbVel = rb.linearVelocity;
        float yVel = rbVel.y; // Capture existing gravity/vertical velocity

        if (direction.magnitude > 0)
        {
            // Apply acceleration to current velocity
            CurrentVelocity = rbVel + (direction * currAcc * Time.deltaTime);
        }
        else
        {
            CurrentVelocity = rbVel;
        }

        // Re-apply gravity so the player doesn't float
        CurrentVelocity.y = yVel;
    }

    void Clamp()
    {
        if (CurrentMovementState == MovementState.Jump) return;

        float verticalVel = CurrentVelocity.y;
        Vector3 horizontalVel = new Vector3(CurrentVelocity.x, 0, CurrentVelocity.z);

        if (IsOnSlope)
        {
            // On slopes, we clamp the total magnitude to prevent "launching" off peaks
            if (CurrentVelocity.magnitude > currMaxVelocity && currMaxVelocity != 0)
            {
                CurrentVelocity = CurrentVelocity.normalized * currMaxVelocity;
            }
        }
        else
        {
            // On flat ground, we only clamp X and Z
            if (horizontalVel.magnitude > currMaxVelocity && currMaxVelocity != 0)
            {
                horizontalVel = horizontalVel.normalized * currMaxVelocity;
                CurrentVelocity = new Vector3(horizontalVel.x, verticalVel, horizontalVel.z);
            }
        }
    }

    void ApplyCrouchHitbox()
    {
        PlayerHitbox.height = Mathf.Lerp(PlayerHitbox.height, data.CrouchHitboxHeight, data.CrouchLerpSpeed * Time.deltaTime);
        GroundTrigger.center = Vector3.Lerp(GroundTrigger.center, data.CrouchGroundCheck, data.CrouchLerpSpeed * Time.deltaTime);
    }

    void ReverseCrouchHitbox()
    {
        PlayerHitbox.height = Mathf.Lerp(PlayerHitbox.height, baseHeight, data.CrouchLerpSpeed * Time.deltaTime);
        GroundTrigger.center = Vector3.Lerp(GroundTrigger.center, standGroundCheck, data.CrouchLerpSpeed * Time.deltaTime);
    }
    #endregion

    #region Physics Detection
    private void OnTriggerStay(Collider other) => IsGround = true;
    private void OnTriggerExit(Collider other) => IsGround = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * RayDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + CurrentVelocity.normalized* RayDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(LowerPos.position, HalfExtend * 2);
        Gizmos.DrawWireCube(UpperPos.position, HalfExtend * 2);
    }
    #endregion
}