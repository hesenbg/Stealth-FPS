using System;
using Unity.VisualScripting;
using UnityEngine;
public class InputManager : MonoBehaviour
{
    private MovementLogic playerMovementLogic;
    private ShootLogic playerShootLogic;
    private ADS ADSlogic;
    private Lean lean;
    private WeaponWallBlock weaponWallBlock;
    private AnimationLogic playerAnimationLogic;
    //[SerializeField] private SoundManager playerSoundManager;

    [Header("Movement Keys")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Movement Variables")]
    Vector3 CurrentDirection;

    [Header("Combat Keys")]
    [SerializeField] private MouseButton ShootKey;
    [SerializeField] private KeyCode ReloadKey;
    [SerializeField] private MouseButton ADS_key;


    [Header("Combat Variables")]
    GunState CurrentGunState;
    enum GunState {Idle, Blocked, Reload, ADS}
   
    private void Start()
    {
        InitilizeMovementVariables();
        InitilizeCombatVariables();
    }
    private void Update()
    {
        UpdateMovement();

        Reload();

    }
    // combat
    void InitilizeCombatVariables()
    {
        CurrentGunState = GunState.Idle;
        ADSlogic = PlayerComponents.Instance.ADS;
        lean = PlayerComponents.Instance.Lean;
        playerShootLogic = PlayerComponents.Instance.ShootLogic;
        weaponWallBlock = PlayerComponents.Instance.WallBlock;
    }

    void Reload()
    {
        if (Input.GetKeyDown(ReloadKey))
        {
            StartCoroutine(playerShootLogic.Reload());
        }
    }


    // movement
    void InitilizeMovementVariables()
    {
        playerMovementLogic = PlayerComponents.Instance.Movement;
        playerAnimationLogic = PlayerComponents.Instance.AnimationLogic;

        playerMovementLogic.OnStepOffGround += OnJumpOffGround;
        playerMovementLogic.OnStepOnGround += OnFallDownGround;
    }

    void TakeInput()
    {
        CurrentDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) CurrentDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) CurrentDirection -= transform.forward;
        if (Input.GetKey(KeyCode.D)) CurrentDirection += transform.right;
        if (Input.GetKey(KeyCode.A)) CurrentDirection -= transform.right;

        CurrentDirection = CurrentDirection.normalized;
    }

    void Idle()
    {
        playerMovementLogic.Idle();
    }
    void OnJumpOffGround(object Sender, EventArgs a)
    {
        PlayerSoundManager.instance.PlayJump();
    }

    void OnFallDownGround(object Sender, EventArgs a)
    {
        PlayerSoundManager.instance.PlayLand();
    }
    void Jump()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            playerMovementLogic.Jump();
        }
    }

    void Walk()
    {
        if (CurrentDirection.sqrMagnitude > 0.1f && !Input.GetKey(sprintKey))
        {
            playerMovementLogic.Walk();
            if (playerMovementLogic.IsGround)
            {
                PlayerSoundManager.instance.PlayWalk();
            }   
        }
    }

    void Run()
    {
        if (Input.GetKey(sprintKey))
        {
            playerMovementLogic.Run();
            if (playerMovementLogic.IsGround)
            {
                PlayerSoundManager.instance.PlayRun();
            }      
        }
    }

    void Crouch()
    {
        bool IsCrouching = Input.GetKey(crouchKey);
        playerMovementLogic.Crouch(IsCrouching);
        playerAnimationLogic.CrouchAnimation(IsCrouching);
    }

    void UpdateMovement()
    {
        TakeInput();
        playerMovementLogic.Direction = CurrentDirection;
        Idle();
        Walk();
        Crouch();
        Jump();
        Run();
    }

}