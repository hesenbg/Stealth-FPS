using UnityEngine;

public class MovementInput : MonoBehaviour
{
    [SerializeField] private MovementLogic playerMovementLogic;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode hookKey = KeyCode.E;

    private void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        playerMovementLogic.MoveInput = dir;

        bool isCrouching = Input.GetKey(crouchKey);
        bool isSprinting = Input.GetKey(sprintKey);

        if (isCrouching)
        {
            playerMovementLogic.Crouch(dir, true);
        }
        else if (dir.magnitude > 0)
        {
            if (isSprinting)
                playerMovementLogic.Walk(dir); 
            else
                playerMovementLogic.Run(dir);  
        }

        if (Input.GetKeyDown(jumpKey))
        {
            playerMovementLogic.Jump();
        }

        if (Input.GetKeyDown(hookKey))
        {

        }
    }





}