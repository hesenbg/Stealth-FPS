using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AnimationLogic : MonoBehaviour
{
    public Animator PlayerAnimator;
    public bool canADS;

    public enum GunState {ADS, Pulled, Idle, Shoot, Reload , Inspect }
    public GunState CurrentGunState;

    /// annimation
    /// 
    /// 
    /// 
    ///
    /// 
    /// 
    /// </summary>

    // Helper to track reloading state without complex animation events
    public bool isReloading = false;
 
    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckReloadState();

        UpdateAnimationVariables();

        PlayMovementAnimations();


        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerAnimator.SetTrigger("Throw");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerAnimator.SetTrigger("Stab");

        }

    }

    void CheckReloadState()
    {
        isReloading = PlayerComponents.Instance.ShootLogic.isReloading;

        

    }

    MovementLogic.MovementState movement;

    void UpdateAnimationVariables()
    {
        movement = PlayerComponents.Instance.Movement.CurrentMovementState;

        // Update movement animations

        bool isWalking = (movement == MovementLogic.MovementState.Walk);

        bool isShooting = PlayerComponents.Instance.ShootLogic.IsShooting;

        bool isIdle = movement == MovementLogic.MovementState.Idle;

        bool isRunning = (movement == MovementLogic.MovementState.Run);

        bool isCrouching = (movement == MovementLogic.MovementState.Crouch);


        if ((isWalking || isShooting || isIdle || isCrouching) && !isReloading && !isRunning)
        {
            canADS = true;
        }
        else
        {
            canADS = false;
        }
    }

    void PlayMovementAnimations()
    {
        PlayerAnimator.SetBool("IsWalking", movement == MovementLogic.MovementState.Walk);
        PlayerAnimator.SetBool("IsRunning", movement == MovementLogic.MovementState.Run);
    }

    public void PlayReloadAnimation(bool IsMagEmpty)
    {
        // We set the trigger, the Update loop will detect the state change
        if (IsMagEmpty)
        {
            PlayerAnimator.SetFloat("ReloadType", 1f);
            PlayerAnimator.SetTrigger("Reload");
        }
        else
        {
            PlayerAnimator.SetFloat("ReloadType", 0f);
            PlayerAnimator.SetTrigger("Reload");
        }
    }

    public void PlayKnifeAttackAnimation() { }

    public void PlayShootAnimation(int CurrentAmmo)
    {
        if (CurrentAmmo == 1)
        {
            PlayerAnimator.SetFloat("ShootType", 0.5f);
            PlayerAnimator.SetTrigger("Shoot");
        }
        else if (CurrentAmmo > 1)
        {
            PlayerAnimator.SetFloat("ShootType", 1f);
            PlayerAnimator.SetTrigger("Shoot");
        }
        else
        {
            PlayerAnimator.SetFloat("ShootType", 0f);
            PlayerAnimator.SetTrigger("Shoot");
        }
    }

}