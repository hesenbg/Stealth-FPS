using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationLogic : MonoBehaviour
{
    public Animator Animator;
    RigBuilder AnimationRig;
    Vector3 CrouchOriginalPosition;

    [SerializeField] float CrouchSpeed;

    public event EventHandler CombatAnimationStart;
    public event EventHandler CombatAnimationEnd;

    private void Start()
    {
        AnimationRig = GetComponent<RigBuilder>();
        Animator = GetComponent<Animator>();  
        CrouchOriginalPosition = PlayerComponents.Instance.MainCamera.transform.localPosition;
    }

    private void Update()
    {
        PlayMovementAnimations();
    }

    void PlayMovementAnimations()
    {
        MovementLogic Player= PlayerComponents.Instance.Movement;

        Animator.SetBool("IsWalking", Player.CurrentMovementState == MovementLogic.MovementState.Walk);
        Animator.SetBool("IsRunning", Player.CurrentMovementState == MovementLogic.MovementState.Run);
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
            Animator.SetFloat("ReloadType", 1f);
            Animator.SetTrigger("Reload");
        }
        else
        {
            Animator.SetFloat("ReloadType", 0f);
            Animator.SetTrigger("Reload");
        }
    }

    public void PlayKnifeAttackAnimation()
    {
        Animator.SetTrigger("Stab");
    }

    public void PlayGrenedeAnimation()
    {
        Animator.SetTrigger("Throw");
    }

    public void StartCombatAnimation()
    {
        CombatAnimationStart?.Invoke(this, EventArgs.Empty);
    }

    public void EndCombatAnimation()
    {
        CombatAnimationEnd?.Invoke(this, EventArgs.Empty);
    }

    public void PlayShootAnimation(int CurrentAmmo)
    {
        if (CurrentAmmo == 1)
        {
            Animator.SetFloat("ShootType", 0.5f);
            Animator.SetTrigger("Shoot");
        }
        else if (CurrentAmmo > 1)
        {
            Animator.SetFloat("ShootType", 1f);
            Animator.SetTrigger("Shoot");
        }
        else
        {
            Animator.SetFloat("ShootType", 0f);
            Animator.SetTrigger("Shoot");
        }
    }

}