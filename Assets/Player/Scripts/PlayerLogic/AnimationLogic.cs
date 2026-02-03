using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public Animator PlayerAnimator;
    public bool canADS;
    // Helper to track reloading state without complex animation events
    public bool isReloading = false;

    Vector3 CrouchOriginalPosition;

    [SerializeField] float Speed;

    private void Start()
    {
        CrouchOriginalPosition = PlayerComponents.Instance.MainCamera.transform.localPosition;
    }

    private void Update()
    {
        PlayMovementAnimations();


        // remove them later
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerAnimator.SetTrigger("Throw");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerAnimator.SetTrigger("Stab");

        }
    }


    MovementLogic.MovementState movement;

    void PlayMovementAnimations()
    {
        PlayerAnimator.SetBool("IsWalking", movement == MovementLogic.MovementState.Walk);
        PlayerAnimator.SetBool("IsRunning", movement == MovementLogic.MovementState.Run);
    }
    
    public void CrouchAnimation(bool IsCrouching)
    {
        if (IsCrouching)
        {
            PlayCrouchAnimation(Speed);
        }
        else
        {
            PlayUnCrouchAnimation(Speed);
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


    void PlayWalkAnimation()
    {
        PlayerAnimator.SetBool("IsWalking", movement == MovementLogic.MovementState.Walk);
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