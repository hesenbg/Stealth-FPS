using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    // --- Movement Parameters ---
    [SerializeField] float JumpForce;
    [SerializeField] public float MaxVelocityGround;
    [SerializeField] float MaxVelocityAir;
    [SerializeField] float GroundAcc;
    [SerializeField] float AirAcc;
    [SerializeField] float CurrMaxVelocity;
    [SerializeField] float CurrAcc;
    [SerializeField] public Vector3 Velocity;

    // --- State ---
    public bool IsGround = false;
    public bool IsMoving = false;

    // --- References ---
    Rigidbody rb;
    [SerializeField] AnimationLogic AnimationLogic;
    [SerializeField] BoxCollider PlayerHitbox;
    [SerializeField] GroundCheck gm;
    [SerializeField] GameObject LowerStep;
    [SerializeField] GameObject UpperStep;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        BaseHitBoxSize = PlayerHitbox.size;
        Cursor.lockState = CursorLockMode.Locked;
    }
    [SerializeField] float CurrentVelocity = 0;
    void HandleHorizontalMovement()
    {
        Velocity = rb.linearVelocity;
        if (Input.GetKey(KeyCode.W))
        {
            Velocity += (transform.forward * CurrAcc * Time.deltaTime) ;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Velocity -= (transform.forward * CurrAcc * Time.deltaTime) ;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Velocity += (transform.right * CurrAcc * Time.deltaTime) ;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Velocity -= (transform.right * CurrAcc * Time.deltaTime) ;
        }
        Clamp();
    }
    Vector3 BaseHitBoxSize;
    public bool IsCrouching= false;
    [SerializeField] float CrouchSpeed = 5f; 
    Vector3 TargetHitBoxSize;

    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            TargetHitBoxSize = new Vector3(0.7f, 1, 0.7f);
            IsCrouching = true;
        }
        else
        {
            TargetHitBoxSize = BaseHitBoxSize;
            IsCrouching = false;
        }
        PlayerHitbox.size = Vector3.Lerp(PlayerHitbox.size, TargetHitBoxSize, Time.deltaTime * CrouchSpeed);
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
    void SetCurrAcc()
    {
        if (IsGround)
        {
            CurrAcc = GroundAcc;
        }
        else
        {
            CurrAcc = AirAcc;
        }
    }
    void SetVelocityBorders()
    {
        if (IsGround)
        {
            CurrMaxVelocity = MaxVelocityGround;
            if (IsCrouching)
            {
                CurrMaxVelocity = MaxVelocityGround / 2;
            }
        }
        else
        {
            CurrMaxVelocity = MaxVelocityAir;
        }
    }
    void SetCurrentVelocity()
    {
        if(Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.D))
        {
            if(CurrentVelocity < CurrMaxVelocity)
            {
                CurrentVelocity+=CurrAcc*Time.deltaTime;
            }
        }
        else
        {
            if(CurrentVelocity >= 0)
            {
                CurrentVelocity -=Time.deltaTime*CurrAcc;
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A))
        {
            if (CurrentVelocity > -CurrMaxVelocity)
            {
                CurrentVelocity -= CurrAcc*Time.deltaTime;
            }
        }
        else
        {
            if (CurrentVelocity <= 0)
            {
                CurrentVelocity += Time.deltaTime;
            }
        }
    }
    void UpdateMovementParameters()
    {
        //SetCurrentVelocity();
        SetCurrAcc();
        SetVelocityBorders();
        Crouch();
    }
    void HandleVerticalMovement()
    {
        if (Input.GetKey(KeyCode.Space) && IsGround)
        {
            Velocity.y = JumpForce;
        }
    }
    public bool IsRunning;
    void SetSituations()
    {
        // is running
        if ((Velocity.z >0.5f || Velocity.z<-0.5f) || (Velocity.x >0.5f || Velocity.x<-0.5f))
        {
            IsMoving = true;
        }
        else
        {
            IsMoving= false;
        }
        if ((Velocity.z > 2f || Velocity.z < -2f) || (Velocity.x > 2f || Velocity.x < -2f))
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }

        if (IsRunning && !IsCrouching)
        {
           //SoundManager.Instance.PlayPlayerFootSteps(transform.position);
        }

        // is pressing ground
        IsGround = gm.IsPressingGround;
    }
    void ApplyVelocity()
    {
        rb.linearVelocity = Velocity;
        
        //Velocity = Vector3.zero;
    }

    void HandleMovement() {
        ClimbStair();
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
    [SerializeField] float MaxStep;
    [SerializeField] LayerMask StairLayer;
    void ClimbStair()
    {
        if (Physics.CheckBox(LowerStep.transform.position, StairCheckExtend, transform.rotation, StairLayer, QueryTriggerInteraction.Ignore))
        {
            if (!Physics.CheckBox(UpperStep.transform.position, StairCheckExtend, transform.rotation, StairLayer, QueryTriggerInteraction.Ignore))
            {
                rb.position -= new Vector3(0,-MaxStep, 0);
            }
        } 
    }
    [SerializeField] Vector3 StairCheckExtend;
    private void OnDrawGizmos()
    {
        if (LowerStep == null || UpperStep == null) return;

        Gizmos.color = Color.yellow;

        // Draw LowerStep cast box
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(LowerStep.transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, StairCheckExtend * 2f);
        Gizmos.matrix = oldMatrix;

        // Draw UpperStep cast box
        Gizmos.matrix = Matrix4x4.TRS(UpperStep.transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, StairCheckExtend * 2f);
        Gizmos.matrix = oldMatrix;
    }

}