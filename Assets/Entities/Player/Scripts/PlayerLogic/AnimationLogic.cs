using System;
using UnityEngine;
public class AnimationLogic : MonoBehaviour
{
    public Animator Animator { get; private set; }
    Vector3 CrouchOriginalPosition;

    [SerializeField] float CrouchSpeed;
    [SerializeField] Transform CrouchObject;
    [SerializeField] float CrouchHeight;

    public event EventHandler CombatAnimationStart;
    public event EventHandler CombatAnimationEnd;
    public event EventHandler ThrowAbleRelease;

    public event EventHandler GunMagOut;
    public event EventHandler GunMagIn;
    public event EventHandler KnifeStab;

    private void Start()
    {
        Animator = GetComponent<Animator>();  
        CrouchOriginalPosition = CrouchObject.localPosition;
    }

    public void PlayMovementAnimations(MovementLogic.MovementState State)
    {
        Animator.SetBool("IsWalking", State == MovementLogic.MovementState.Walk || State == MovementLogic.MovementState.Crouch);
        Animator.SetBool("IsRunning", State == MovementLogic.MovementState.Run);
    }


    public void CrouchAnimation(bool IsCrouching)
    {
        Animator.SetBool("IsWalking", IsCrouching);

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
            , CrouchHeight
            , CrouchOriginalPosition.z);

        CrouchObject.localPosition = Vector3.Lerp(CrouchObject.localPosition, CrouchPos, Time.deltaTime*Speed);
    }

    void PlayUnCrouchAnimation(float Speed)
    {
        CrouchObject.localPosition = Vector3.Lerp(CrouchObject.localPosition, CrouchOriginalPosition, Time.deltaTime * Speed);
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

    public void PlayShootAnimation(int CurrentAmmo)
    {
        // fixed animation
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

        // procedural animation
    }
    public void PlayGrenedeAnimation(float NadeThrowType)  // 1 means long and 0 means short
    {
        Animator.SetFloat("ThrowType", NadeThrowType);
        Animator.SetTrigger("Throw");
    }

    // event fires

    public void FireKnifeStab()
    {
        KnifeStab?.Invoke(this,EventArgs.Empty);
    }

    public void FireThrowAbleRelease()
    {
        ThrowAbleRelease?.Invoke(this, EventArgs.Empty);
    }

    public void StartCombatAnimation()
    {
        CombatAnimationStart?.Invoke(this, EventArgs.Empty);
    }

    public void EndCombatAnimation()
    {
        CombatAnimationEnd?.Invoke(this, EventArgs.Empty);
    }

    public void FireMagOut()
    {
        GunMagOut?.Invoke(this, EventArgs.Empty);
    }

    public void FireMagIn()
    {
        GunMagIn?.Invoke(this, EventArgs.Empty);
    }
}