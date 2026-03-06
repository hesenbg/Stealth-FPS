using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InputManager : MonoBehaviour
{
    public enum InputType { Hold, Toggle }

    [Header("References")]
    [SerializeField] private MovementLogic playerMovementLogic;
    [SerializeField] private ShootLogic playerShootLogic;
    [SerializeField] private ADS ADSlogic;
    [SerializeField] private WeaponWallBlock weaponWallBlock;
    [SerializeField] private AnimationLogic playerAnimationLogic;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private ThrowAbleLogic playerThrowAbleLogic;
    [SerializeField] private Knife playerKnife;
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

    [Header("Settings")]
    [SerializeField] private InputType adsInputType = InputType.Hold;

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

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    void InitilizeCombatVariables()
    {
        CurrentGunState = GunState.Idle;
        ADSlogic = PlayerComponents.Instance.ADS;
        playerShootLogic = PlayerComponents.Instance.ShootLogic;
        weaponWallBlock = PlayerComponents.Instance.WallBlock;
        playerUI = PlayerComponents.Instance.PlayerUI;
        playerThrowAbleLogic = PlayerComponents.Instance.ThrowAbleLogic;

        playerShootLogic.OnReloadEnd += OnReloadEnd;
        playerAnimationLogic.GunMagOut += OnReloadMagOut;
        playerAnimationLogic.GunMagIn += OnReloadMagIn;

        weaponWallBlock.WallBlockEnd += OnBlockEnd;
        weaponWallBlock.WallBlockStart += OnBlockStart;

        playerAnimationLogic.CombatAnimationEnd += OnBlockEnd;
        playerAnimationLogic.CombatAnimationStart += OnBlockStart;
        playerAnimationLogic.KnifeStab += OnKNifeStab;

        playerAnimationLogic.ThrowAbleRelease += OnNadeThrown;
    }

    void ADS()
    {
        if (adsInputType == InputType.Hold)
        {
            HandleHoldADS();
        }
        else
        {
            HandleToggleADS();
        }

        if (CurrentGunState == GunState.ADS)
        {
            ADSlogic.ApplyADS();
        }

        if (CurrentGunState == GunState.Idle)
        {
            ADSlogic.RevertADS();
        }
    }

    void HandleHoldADS()
    {
        if (Input.GetMouseButtonUp(1))
        {
            CurrentGunState = GunState.Idle;
        }

        if (Input.GetMouseButton(1) && (CurrentGunState == GunState.Idle || CurrentGunState == GunState.ADS))
        {
            CurrentGunState = GunState.ADS;
        }
    }

    void HandleToggleADS()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (CurrentGunState == GunState.Idle)
            {
                CurrentGunState = GunState.ADS;
            }
            else if (CurrentGunState == GunState.ADS)
            {
                CurrentGunState = GunState.Idle;
            }
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && (CurrentGunState == GunState.Idle || CurrentGunState == GunState.ADS))
        {
            if (!playerShootLogic.CanShoot())
                return;
            playerShootLogic.CalculateRecoil();

            playerShootLogic.Shoot();

            PlayerRecoil.RecoilFire(CurrentGunState == GunState.ADS);

            playerShootLogic.CalculateRecoilDaper(CurrentGunState == GunState.ADS, playerMovementLogic.CurrentVelocity.magnitude);

            playerAnimationLogic.PlayShootAnimation(playerShootLogic.CurrentMagazineAmmo);

            PlayerSoundManager.instance.PlayShootSound();
        }
        playerUI.UpdateGunUI(playerShootLogic.CurrentMagazineAmmo, playerShootLogic.CurrentTotalAmmo);
    }

    void Reload()
    {
        if (Input.GetKeyDown(ReloadKey) && CurrentGunState == GunState.Idle)
        {
            CurrentGunState = GunState.Reload;
            StartCoroutine(playerShootLogic.Reload());
            playerAnimationLogic.PlayReloadAnimation(playerShootLogic.CurrentMagazineAmmo == 0);
        }
    }

    void OnReloadMagOut(object sender, EventArgs a)
    {
        playerShootLogic.MagOut();
        PlayerSoundManager.instance.PlayMagOut();
    }

    void OnReloadMagIn(object sender, EventArgs a)
    {
        playerShootLogic.MagIn();
        PlayerSoundManager.instance.PlayMagIn();
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
            float controlValue = Input.GetKey(KeyCode.LeftControl) ? 0f : 1f;
            playerAnimationLogic.PlayGrenedeAnimation(controlValue);
        }
    }

    void OnKNifeStab(object sender, EventArgs a)
    {
        playerKnife.Damage();
    }

    void OnNadeThrown(object sender, EventArgs e)
    {
        playerThrowAbleLogic.ThrowNadeLong();
    }

    void OnReloadEnd(object sender, EventArgs a)
    {
        CurrentGunState = GunState.Idle;
    }

    void OnBlockStart(object sender, EventArgs a)
    {
        CurrentGunState = GunState.Blocked;
    }

    void OnBlockEnd(object sender, EventArgs a)
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
        if (Input.GetKey(sprintKey) && CurrentDirection.magnitude > 0.01f)
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
        Crouch();
        Idle();
        Walk();
        Jump();
        Run();
        playerAnimationLogic.PlayMovementAnimations(playerMovementLogic.CurrentMovementState);
    }
}