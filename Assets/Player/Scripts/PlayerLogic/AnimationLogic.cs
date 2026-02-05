using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public Animator PlayerAnimator;

    Vector3 CrouchOriginalPosition;

    [SerializeField] float CrouchSpeed;

    private void Start()
    {
        CrouchOriginalPosition = PlayerComponents.Instance.MainCamera.transform.localPosition;
    }

    private void Update()
    {
        PlayMovementAnimations();
    }

    void PlayMovementAnimations()
    {
        MovementLogic Player= PlayerComponents.Instance.Movement;

        PlayerAnimator.SetBool("IsWalking", Player.CurrentMovementState == MovementLogic.MovementState.Walk);
        PlayerAnimator.SetBool("IsRunning", Player.CurrentMovementState == MovementLogic.MovementState.Run);
    }
    
    public void CrouchAnimation(bool IsCrouching)
    {
        if (IsCrouching)
        {
            PlayCrouchAnimation(CrouchSpeed);
        }
        else
        {
            PlayUnCrouchAnimation(CrouchSpeed);
        }
    }
    

    void PlayCrouchAnimation(float Speed)
    {
        Vector3 CrouchPos = new Vector3(
             CrouchOriginalPosition.x
            , 0
            , CrouchOriginalPosition.z);

        Transform cam = PlayerComponents.Instance.MainCamera.transform;

        cam.localPosition = Vector3.Lerp(cam.localPosition, CrouchPos, Time.deltaTime*Speed);
    }

    void PlayUnCrouchAnimation(float Speed)
    {
        Transform cam = PlayerComponents.Instance.MainCamera.transform;

        cam.localPosition = Vector3.Lerp(cam.localPosition, CrouchOriginalPosition, Time.deltaTime * Speed);
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

    public void PlayKnifeAttackAnimation()
    {

    }

    public void PlayGrenedeAnimation()
    {

    }

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