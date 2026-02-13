using System;
using Unity.VisualScripting;
using UnityEngine;
public class InputManager : MonoBehaviour
{
    [Header("References")]
    private MovementLogic playerMovementLogic;
    private ShootLogic playerShootLogic;
    private ADS ADSlogic;
    private WeaponWallBlock weaponWallBlock;
    private AnimationLogic playerAnimationLogic;

    [SerializeField] Recoil PlayerRecoil;

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
    [SerializeField] private KeyCode KnifeStabKey;
    [SerializeField] private KeyCode ThrowObjectKey;

    [Header("Combat Variables")]
    public GunState CurrentGunState;
    public enum GunState { Idle, Blocked, Reload, ADS }
   
    private void Start()
    {
        InitilizeMovementVariables();
        InitilizeCombatVariables();
    }
    private void Update()
    {
        UpdateMovement();
        UpdateCombat();
    }

    void InitilizeCombatVariables()
    {
        // variable assigning
        CurrentGunState = GunState.Idle;
        ADSlogic = PlayerComponents.Instance.ADS;
        playerShootLogic = PlayerComponents.Instance.ShootLogic;
        weaponWallBlock = PlayerComponents.Instance.WallBlock;
        // event subscribing
        // reload
        playerShootLogic.OnReloadEnd += OnReloadEnd;

        // weapon block
        weaponWallBlock.WallBlockEnd += OnBlockEnd;
        weaponWallBlock.WallBlockStart += OnBlockStart;

        playerAnimationLogic.CombatAnimationEnd += OnBlockEnd;
        playerAnimationLogic.CombatAnimationStart += OnBlockStart;
    }

    void ADS()
    {
        if (Input.GetMouseButtonUp(1))
        {
            CurrentGunState = GunState.Idle;
        }
        if (Input.GetMouseButton(1) && CurrentGunState != GunState.Blocked)
        {
            CurrentGunState = GunState.ADS;
            ADSlogic.ApplyADS();
        }
        if(CurrentGunState == GunState.Idle)
        {
            ADSlogic.RevertADS();
        }
    }

    void Shoot()
    {
        if(Input.GetMouseButton(0) && (CurrentGunState== GunState.Idle || CurrentGunState == GunState.ADS))
        {
            if (!playerShootLogic.CanShoot())
                return;

            playerShootLogic.Shoot();

            playerShootLogic.CalculateRecoil();

            PlayerRecoil.RecoilFire(playerShootLogic.TotalCurrentRecoil);

            playerAnimationLogic.PlayShootAnimation(playerShootLogic.CurrentMagazineAmmo);

            PlayerSoundManager.instance.PlayShootSound();
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(ReloadKey) && CurrentGunState== GunState.Idle)
        {
            CurrentGunState = GunState.Reload;
            StartCoroutine(playerShootLogic.Reload());
            playerAnimationLogic.PlayReloadAnimation(playerShootLogic.CurrentMagazineAmmo==0);
        }
    }

    void KnifeStab()
    {
        if (Input.GetKeyDown(KnifeStabKey) && CurrentGunState == GunState.Idle)
        {
            CurrentGunState = GunState.Blocked;
            playerAnimationLogic.PlayKnifeAttackAnimation();
        }
    }

    void ThrowObject()
    {
        if (Input.GetKeyDown(ThrowObjectKey) && CurrentGunState == GunState.Idle)
        {
            CurrentGunState = GunState.Blocked;
            playerAnimationLogic.PlayGrenedeAnimation();
        }
    }

    void OnReloadEnd(object  sender,EventArgs a)
    {
        CurrentGunState = GunState.Idle;
    }

    void OnBlockStart(object sender,EventArgs a)
    {
        CurrentGunState = GunState.Blocked;
    }

    void OnBlockEnd(object sender,EventArgs a)
    {
        CurrentGunState = GunState.Idle;
    }

    void UpdateCombat()
    {
        Reload();
        ThrowObject();
        KnifeStab();    
        Shoot();
        ADS();
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