using UnityEngine;
using UnityEngine.UIElements;

public class AnimationLogic : MonoBehaviour
{
    public Animator PlayerAnimator;
    public bool canADS;

    // Helper to track reloading state without complex animation events
    public bool isReloading = false;

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerData.SetAnimationLogic(this);
    }

    private void Update()
    {
        // 1. Check if the reload animation has finished playing
        CheckReloadState();

        // 2. Update logic
        UpdateAnimationVariables();
    }

    void CheckReloadState()
    {
        isReloading =PlayerData.GetShootLogic().isReloading;
    }

    void UpdateAnimationVariables()
    {
        var movement = PlayerData.GetMovement().CurrentMovementState;

        // Update movement animations
        PlayerAnimator.SetBool("IsWalking", movement == PlayerMovement.MovementState.Walk);
        PlayerAnimator.SetBool("IsRunning", movement == PlayerMovement.MovementState.Run);


        bool isWalking = (movement == PlayerMovement.MovementState.Walk);


        bool isShooting = PlayerData.GetShootLogic().IsShooting;

        bool isIdle = movement == PlayerMovement.MovementState.Idle;

        bool isRunning = (movement == PlayerMovement.MovementState.Run);

        bool isCrouching = (movement == PlayerMovement.MovementState.Crouch);


        if ((isWalking || isShooting || isIdle || isCrouching) && !isReloading && !isRunning)
        {
            canADS = true;
        }
        else
        {
            canADS = false;
        }
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

    public void UpdateEnemyAnimations() { }
}