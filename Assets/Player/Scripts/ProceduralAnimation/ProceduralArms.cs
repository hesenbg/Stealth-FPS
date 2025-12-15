using UnityEngine;

public class ProceduralArms : MonoBehaviour
{
    [Header("Mouse Sway")]
    [SerializeField] float Sway ;
    [SerializeField] float Speed ;
    [SerializeField] float MaxSway ;

    [Header("Movement Bobbing")]
    [SerializeField] float MoveSwayAmount;
    [SerializeField] float MaxMoveSway ;
    [SerializeField] float bobSpeed;

    [SerializeField] float JumpBobMultipler;

    Vector2 MouseMovement;
    Vector3 PhysicalMovement;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        GetInput();
        ApplySway();
        MoveSway();
    }

    void GetInput()
    {
        // Mouse
        MouseMovement.x = Mathf.Clamp(Input.GetAxisRaw("Mouse X") * Sway, -MaxSway, MaxSway);
        MouseMovement.y = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * Sway, -MaxSway, MaxSway);

        // Movement (WASD)
        PhysicalMovement.x = Input.GetAxisRaw("Horizontal");
        PhysicalMovement.y = Input.GetAxisRaw("Vertical");
    }

    void ApplySway()
    {
        Quaternion rotX = Quaternion.AngleAxis(-MouseMovement.y, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(MouseMovement.x, Vector3.up);
        Quaternion targetRot = rotX * rotY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Speed * Time.deltaTime);
    }

    void MoveSway()
    {
        // Calculate how much input is being pressed
        float moveX = Mathf.Clamp(PhysicalMovement.x * MoveSwayAmount*Time.deltaTime, -MaxMoveSway, MaxMoveSway);
        float moveZ = Mathf.Clamp(PhysicalMovement.y * MoveSwayAmount*Time.deltaTime, -MaxMoveSway, MaxMoveSway);
        float moveY = Mathf.Clamp((PlayerData.GetMovement().Velocity.y / PlayerData.GetMovement().JumpForce) * MoveSwayAmount*JumpBobMultipler * Time.deltaTime, -MaxMoveSway, MaxMoveSway);

        Vector3 targetPos = startPos + new Vector3(-moveX, moveY, -moveZ);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * bobSpeed);
        
    }

}
