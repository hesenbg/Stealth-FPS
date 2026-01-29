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
        Vector3 currentDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) currentDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) currentDirection -= transform.forward;
        if (Input.GetKey(KeyCode.D)) currentDirection += transform.right;
        if (Input.GetKey(KeyCode.A)) currentDirection -= transform.right;

        currentDirection.Normalize();

        playerMovementLogic.Direction = currentDirection;

        playerMovementLogic.Idle();

        if (Input.GetKeyDown(jumpKey))
        {
            playerMovementLogic.Jump();
        }

        playerMovementLogic.Hook(Input.GetKey(hookKey));
        
        if (Input.GetKey(sprintKey))
        {
            playerMovementLogic.Run();
        }
        else if(currentDirection.sqrMagnitude > 0.1f)
        {
            playerMovementLogic.Walk();
        }

        playerMovementLogic.Crouch(Input.GetKey(crouchKey));
    }
}