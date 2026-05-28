using System;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAnimationLogic : MonoBehaviour
{
    Animator animator;

    [SerializeField] CapsuleCollider BaseHitBox;
    [SerializeField] CapsuleCollider CrouchHitBox;
    [SerializeField] float blendDampTime = 0.1f;

    public event EventHandler InvestigationEnd;
    public enum MovementState {Idle, Walk, Crouch }
    public enum UpperBodyState { Idle, PistolHold, LookAround,Walk}

    private static readonly Dictionary<MovementState, float> moveBlendValues = new()
    {
        { MovementState.Crouch, 0f },
        { MovementState.Idle,   0.5f },
        { MovementState.Walk,   1f }
    };
    private static readonly Dictionary<UpperBodyState, float> upperBodyBlendValues = new()
    {
        { UpperBodyState.Idle,        0f   },
        { UpperBodyState.PistolHold,  0.5f },
        { UpperBodyState.LookAround,  1f   },
        { UpperBodyState.Walk,        1.5f   }
    };

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void PlayMovementAnimation(MovementState state)
    {
        animator.SetFloat("MoveBlend", moveBlendValues[state], blendDampTime, Time.deltaTime);
    }
    private void PlayUpperBodyAnimation(UpperBodyState state)
    {
        animator.SetFloat("UpperBodyBlend", upperBodyBlendValues[state], blendDampTime, Time.deltaTime);
    }

    public void PlayIdlePistol()
    {
        WholeBody(MovementState.Idle,UpperBodyState.PistolHold);
    }
    public void PlayWalkPistol()
    {
        WholeBody(MovementState.Walk, UpperBodyState.PistolHold);
    }

    public void PlayIdleLookAround()
    {
        WholeBody(MovementState.Idle, UpperBodyState.LookAround);
    }


    public void PlayWalk()
    {
        WholeBody(MovementState.Walk, UpperBodyState.Walk);
    }

    public void PlayIdle()
    {
        WholeBody(MovementState.Idle, UpperBodyState.Idle);
    }

    private void WholeBody(MovementState moveState,UpperBodyState upperState)
    {
        if(moveState == MovementState.Crouch)
        {
            BaseHitBox.enabled = false;
            CrouchHitBox.enabled = true;
        }
        else
        {
            BaseHitBox.enabled = true;
            CrouchHitBox.enabled = false;
        }

        PlayUpperBodyAnimation(upperState);
        PlayMovementAnimation(moveState);
    }

    public void FireInvestigationEnd()
    {
        InvestigationEnd?.Invoke(this, EventArgs.Empty);
    }
}