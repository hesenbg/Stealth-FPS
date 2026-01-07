using System.Collections;
using UnityEngine;
public  class PlayerMovement : MonoBehaviour
{
    [Header("General Paramaters")]
    [SerializeField] float CurrMaxVelocity;
    [SerializeField] float CurrAcc;
    [SerializeField] public Vector3 Velocity;
    [SerializeField] float JumpMass;
    [SerializeField] public float JumpForce;

    [Header("Max movementState Speeds")]
    [SerializeField] float WalkSpeed;
    [SerializeField] float RunSpeed;
    [SerializeField] float CrouchSpeed;

    [Header("movement state Accelerations")]
    [SerializeField] float WalkAcceleartion;
    [SerializeField] float RunAcceleration;
    [SerializeField] float CrouchAcceleration;
    [SerializeField] float JumpAcceleration;

    [Header("Crouch Hitbox (Capsule)")]
    float TargetHeight;
    float BaseHeight;
    [SerializeField] float CrouchHitboxHeight;

    [SerializeField] float CrouchLerpSpeed = 10f;

    Vector3 StandGroundCheck;
    Vector3 TargetGroundCheck;
    [SerializeField] Vector3 CrouchGroundCheck;

    public bool IsGround = false;
    public bool IsOnSlope = false;
    [Header("Movement States")]
    public MovementState CurrentMovementState;
    public enum MovementState {Walk, Run, Crouch, Jump, Idle}

    [Header("Obstacle Avoidance")]
    [SerializeField] Transform LowerPos;
    [SerializeField] Transform UpperPos;
    [SerializeField] Vector3 HalfExtend;
    [SerializeField] Vector3 RigidbodyUp;
    [SerializeField] LayerMask ObstacleMask;

    [Header("References")]
    Rigidbody rb;
    CapsuleCollider PlayerHitbox;
    BoxCollider GroundTrigger; // detects if player on slope, ground or on air
    GameObject Surface;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        PlayerHitbox = GetComponent<CapsuleCollider>();

        BaseHeight = PlayerHitbox.height;

        StandGroundCheck = GroundTrigger.center;
    }

    void HandleHorizontalMovement() // calcuates the velocity based on the imput
    {
        Velocity = rb.linearVelocity;

        if (Input.GetKey(KeyCode.W))
        {
            Velocity += (CurrentForwardDirection * CurrAcc * Time.deltaTime) ;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Velocity -= (CurrentForwardDirection * CurrAcc * Time.deltaTime) ;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Velocity += (CurrentRightDirection * CurrAcc * Time.deltaTime) ;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Velocity -= (CurrentRightDirection * CurrAcc * Time.deltaTime) ;
        }
        UpdateDirection();
    }

    Vector3 CurrentForwardDirection;
    Vector3 CurrentRightDirection;

    // updates directions (forward and right) based on the nomral of surface
    void UpdateDirection()
    {
        if (IsOnSlope)
        {
            rb.useGravity = false;
            rb.linearDamping = 4f;
            CurrentForwardDirection = Vector3.ProjectOnPlane(transform.forward, Surface.transform.up).normalized;
            CurrentRightDirection = Vector3.ProjectOnPlane(transform.right, Surface.transform.up).normalized;
        }
        else
        {
            rb.useGravity = true;
            CurrentRightDirection = transform.right;
            CurrentForwardDirection = transform.forward;
        }
    }


    // slecect the current state
    void UpdateCurrentMovementState()
    {
        // jump
        if (!IsGround && !IsOnSlope)
        {
            CurrentMovementState = MovementState.Jump;
            return;
        }

        // crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            CurrentMovementState = MovementState.Crouch;
            return;
        }

        // Check if any movement key is pressed
        bool moving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        if (moving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                CurrentMovementState = MovementState.Walk;
            else
                CurrentMovementState = MovementState.Run;

            return;
        }

        // 4. If no movement input  Idle
        CurrentMovementState = MovementState.Idle;
    }

    void SetMovementParametersFromState()
    {
        switch (CurrentMovementState)
        {
            case MovementState.Walk:
                CurrAcc = WalkAcceleartion;
                CurrMaxVelocity = WalkSpeed;
                break;

            case MovementState.Run:
                CurrAcc = RunAcceleration;
                CurrMaxVelocity = RunSpeed;
                break;

            case MovementState.Crouch:
                CurrAcc = CrouchAcceleration;
                CurrMaxVelocity = CrouchSpeed;
                break;

            case MovementState.Jump:
                // Keep your horizontal movement while in the air
                CurrMaxVelocity = JumpForce;
                CurrAcc = JumpAcceleration;
                break;

            case MovementState.Idle: // Idle
                CurrAcc = 0f;
                //CurrMaxVelocity = 0f;
                break;
        }
    }

    void Clamp()
    {

        // while jumping movement doesnt clamp
        if(CurrentMovementState == MovementState.Jump)
        {
            return;
        }
        Vector3 ClampVelocity;
        if (IsOnSlope )
        {
            ClampVelocity = Velocity;

            // clamp only horizontal
            if (ClampVelocity.magnitude > CurrMaxVelocity)
            {
                ClampVelocity = ClampVelocity.normalized * CurrMaxVelocity;
                
            }

            Velocity = ClampVelocity;
            return;
        }

        ClampVelocity = Velocity;
        // clamp horizontal speed to CurrMaxVelocity
        ClampVelocity = new Vector3(Velocity.x, 0, Velocity.z);
        if (ClampVelocity.magnitude > CurrMaxVelocity && CurrMaxVelocity!=0)
        {
            ClampVelocity = ClampVelocity.normalized * CurrMaxVelocity;
            Velocity.x = ClampVelocity.x;
            Velocity.z = ClampVelocity.z;

        }
    }
    void Crouch()
    {
        
        bool isCrouch = Input.GetKey(KeyCode.LeftControl);

        if (isCrouch)
        {
            TargetGroundCheck = CrouchGroundCheck;
            TargetHeight = CrouchHitboxHeight;
        }
        else
        {
            TargetHeight = BaseHeight;
            TargetGroundCheck = StandGroundCheck;
        }

        PlayerHitbox.height = Mathf.Lerp(PlayerHitbox.height, TargetHeight, CrouchLerpSpeed * Time.deltaTime);
        GroundTrigger.center = Vector3.Lerp(GroundTrigger.center, TargetGroundCheck, CrouchLerpSpeed * Time.deltaTime);
    }

    // this should start a coroutine and increase Y velocity with the acceleration
    void HandleVerticalMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            Velocity += JumpForce * transform.up ;
        }
        if (IsGround)
        {
            rb.mass = 1;
        }
        else
        {
            rb.mass = JumpMass;
        }
    }

    IEnumerator Jump()
    {

        yield return null;
    }

    void UpdateMovementParameters()
    {
        UpdateCurrentMovementState();
        SetMovementParametersFromState();
        Crouch();
    }

    void SetSituations()
    {

    }

    void ApplyVelocity()
    {
        rb.linearVelocity = Velocity;
        
        //Velocity = Vector3.zero;
    }

    void CheckObstacle()
    {
        if(Physics.OverlapBox(LowerPos.position, HalfExtend,transform.rotation,ObstacleMask).Length >= 1)
        {
            if (Physics.OverlapBox(UpperPos.position, HalfExtend, transform.rotation, ObstacleMask).Length == 0)
            {
                rb.position = Vector3.Lerp(rb.position, rb.position + RigidbodyUp, Time.deltaTime * CrouchLerpSpeed);
            }
        }
    }

    void HandleMovement()
    {
        UpdateDirection();
        HandleHorizontalMovement();
        HandleVerticalMovement();
        Clamp();
        ApplyVelocity();
    }


    private void Update()
    {
        UpdateMovementParameters();
        HandleMovement();
        SetSituations();
        CheckObstacle();
    }
    private void OnDrawGizmos()
    {
           Gizmos.DrawCube(LowerPos.position, HalfExtend);
           Gizmos.DrawCube(UpperPos.position, HalfExtend);
        if (rb == null) return;
           Gizmos.color = Color.yellow;
           Gizmos.DrawLine(rb.position, rb.position + rb.linearVelocity);
    }

    private void OnTriggerStay(Collider other)
    {
        Surface = other.gameObject;

        if(other.gameObject.layer== 13)
        {
            IsOnSlope = true;
        }

        IsGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsGround = false;

        if (other.gameObject.layer == 13)
        {
            IsOnSlope = false;
        }
    }
}