using System.Collections;
using UnityEngine;
public  class PlayerMovement : MonoBehaviour
{
    [Header("General Paramaters")]
    [SerializeField] float CurrMaxVelocity;
    [SerializeField] float CurrAcc;
    [SerializeField] public Vector3 Velocity;
    [SerializeField] float JumpMass;
    [SerializeField] float JumpForce;

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

    [Header("References")]
    Rigidbody rb;
    [SerializeField] AnimationLogic AnimationLogic;
    [SerializeField] CapsuleCollider PlayerHitbox;
    [SerializeField] BoxCollider GroundTrigger; // detects if player on slope, ground or on air
    GameObject Surface;
    /// <movement keybinds>
    /// 
    /// w a s d alone means running
    ///  
    /// w a d d + shift means walking
    /// 
    /// ctrl means crouching
    /// 
    /// spacebar means jumping

    void Start()
    {
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
        Clamp();

        UpdateDirection();
    }

    Vector3 CurrentForwardDirection;
    Vector3 CurrentRightDirection;

    // updates directions (forward and right) based on the nomral of surface
    void UpdateDirection()
    {
        if (IsOnSlope)
        {

        }
    }


    // slecect the current state
    void UpdateCurrentMovementState()
    {
        if (IsOnSlope)
        {
            rb.useGravity = false;
            CurrentForwardDirection = Surface.transform.forward;
            CurrentRightDirection = Surface.transform.right;
        }
        else
        {
            rb.useGravity = true;
            CurrentForwardDirection = transform.forward;
            CurrentRightDirection = transform.right;
        }

        // jump
        if (!IsGround)
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
                CurrAcc = JumpAcceleration;
                break;

            default: // Idle
                CurrAcc = 0f;
                CurrMaxVelocity = 0f;
                break;
        }
    }

    void Clamp()
    {
        // clamp horizontal speed to CurrMaxVelocity
        Vector3 horizontal = new Vector3(Velocity.x, 0, Velocity.z);
        if (horizontal.magnitude > CurrMaxVelocity && CurrMaxVelocity!=0)
        {
            horizontal = horizontal.normalized * CurrMaxVelocity;
            Velocity.x = horizontal.x;
            Velocity.z = horizontal.z;
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
        if (Input.GetKey(KeyCode.Space) && IsGround)
        {
            Velocity.y = JumpForce;
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

    void HandleMovement() {
        HandleHorizontalMovement();
        HandleVerticalMovement();
        ApplyVelocity();
    }

    private void Update()
    {
        UpdateMovementParameters();
        HandleMovement();
        SetSituations();
    }

    private void OnDrawGizmos()
    {
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

        if(other.gameObject.layer == 6)
        {
            IsGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Surface = null;

        if (other.gameObject.layer == 6)
        {
            IsGround = false;
        }

        if (other.gameObject.layer == 13)
        {
            IsOnSlope = false;
        }
    }
}