using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static AwarenessFSM;

public class EnemyAnimationLogic : MonoBehaviour
{
    Animator animator;

    [SerializeField] CapsuleCollider BaseHitBox;
    [SerializeField] CapsuleCollider CrouchHitBox;
    [SerializeField] float blendDampTime = 0.1f;

    [SerializeField] EnemyStateMachine fsm;

    [SerializeField] float CurrentSpeed;

    [SerializeField] AwarenessFSM awareness;

    NavMeshAgent agent;

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

    private void Start()
    {
        fsm = GetComponentInParent<EnemyStateMachine>();

        awareness = GetComponentInParent<AwarenessFSM>();

        PlayIdle();

        agent = fsm.context.agent;
    }


    private void Update()
    {
        CurrentSpeed = agent.velocity.magnitude;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if(awareness.CurrentAwarnessState == AwarnessState.Idle)
        {
            UpdateIdle();
        }
        else  if(awareness.CurrentAwarnessState == AwarnessState.Suspicious || awareness.CurrentAwarnessState == AwarnessState.Alarmed)
        {

            UpdatePistol(fsm.context.enemyAIData.IsHiding);
            
        }
    }

    void UpdatePistol(bool IsPeeking)
    {
        if (IsPeeking)
        {
            PlayCrouchPistol();
            return;
        }

        if (CurrentSpeed < 0.1f)
        {
            PlayIdlePistol();
        }
        else
        {
            PlayWalkPistol();
        }
    }


    void UpdateIdle()
    {
        if(CurrentSpeed < 0.1f)
        {
            PlayIdle();
        }
        else
        {
            PlayWalk();
        }
    }

    private void PlayMovementAnimation(MovementState state)
    {
        animator.SetFloat("MoveBlend", moveBlendValues[state], blendDampTime, Time.deltaTime);
    }
    private void PlayUpperBodyAnimation(UpperBodyState state)
    {
        animator.SetFloat("UpperBodyBlend", upperBodyBlendValues[state], blendDampTime, Time.deltaTime);
    }



    private void PlayIdle()
    {
        WholeBody(MovementState.Idle, UpperBodyState.Idle);
    }

    private void PlayWalk()
    {
        WholeBody(MovementState.Walk, UpperBodyState.Walk);
    }

    private void PlayIdlePistol()
    {
        WholeBody(MovementState.Idle, UpperBodyState.PistolHold);
    }

    private void PlayWalkPistol()
    {
        WholeBody(MovementState.Walk, UpperBodyState.PistolHold);
    }

    private void PlayCrouchPistol()
    {
        WholeBody(MovementState.Crouch, UpperBodyState.PistolHold);
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