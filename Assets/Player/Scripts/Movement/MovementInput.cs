using System;
using UnityEngine;
public class MovementInput : MonoBehaviour
{
    [SerializeField] private MovementLogic playerMovementLogic;
    [SerializeField] private AnimationLogic playerAnimationLogic;
    //[SerializeField] private SoundManager playerSoundManager;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    Vector3 CurrentDirection;

    private void Start()
    {
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
        if (Input.GetKey(sprintKey))
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

    private void Update()
    {
        TakeInput();

        playerMovementLogic.Direction = CurrentDirection;

        Idle();
        Walk();
        Crouch();
        Jump();
        Run();
    }
}