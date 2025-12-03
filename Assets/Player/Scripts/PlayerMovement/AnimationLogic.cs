using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public Animator PlayerAnimator;


    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerData.SetAnimationLogic(this);
    }

    private void Update()
    {
        UpdateAnimationVariables();
    }

    void UpdateAnimationVariables()
    {
        PlayerAnimator.SetBool("IsWalking", PlayerData.GetMovement().CurrentMovementState == PlayerMovement.MovementState.Walk);
        PlayerAnimator.SetBool("IsRunning", PlayerData.GetMovement().CurrentMovementState == PlayerMovement.MovementState.Run);
    }

    public void PlayReloadAnimation(bool IsMagEmpty)
    {
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

    public void PlayKnifeAttackAnimation()
    {
        
    }

    public void PlayShootAnimation(int CurrentAmmo)
    {
        if (CurrentAmmo == 1)
        {
            PlayerAnimator.SetFloat("ShootType", 0.5f);
            PlayerAnimator.SetTrigger("Shoot");
        }
        else if(CurrentAmmo > 1)
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

    public void UpdateEnemyAnimations()
    {

    }
}
