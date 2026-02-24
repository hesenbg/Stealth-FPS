using UnityEngine;

public class ProceduralArms : MonoBehaviour
{
    [Header("Mouse Sway")]
    [SerializeField] float swayAmount = 1.5f;
    [SerializeField] float swaySpeed = 8f;
    [SerializeField] float maxSway = 2f;

    [Header("Movement Bobbing")]
    [SerializeField] float moveSwayAmount = 0.05f;
    [SerializeField] float maxMoveSway = 0.1f;
    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float jumpBobMultiplier = 2f;
    [SerializeField] float MoveMultipiler;

    [Header("Lean Settings")]
    [SerializeField] float maxLeanAngle = 15f;
    [SerializeField] float maxCameraXOffset = 0.25f;
    [SerializeField] float leanSpeed = 8f;
    [SerializeField] float leanThreshold = 0.01f;

    public enum LeanMode { Toggle, Hold }
    [SerializeField] LeanMode currentLeanMode;

    [Header("References")]
    [SerializeField] MovementData playerMovementData;
    [SerializeField] Transform PlayerMesh;

    [Header("Check")]
    [SerializeField] Transform CheckOrigin;
    [SerializeField] float leanCheckRadius = 0.25f;
    [SerializeField] float leanCheckDistance = 0.5f;
    public bool IsRightBlocked;
    public bool IsLeftBlocked;

    Vector3 meshStartLocalPos;
    Vector3 StartLocalPos;
    Vector2 mouseMovement;
    Vector2 physicalMovement;
    float currentLeanAngle;
    float targetLeanAngle;

    void Start()
    {
        if (PlayerMesh != null) meshStartLocalPos = PlayerMesh.localPosition;
    }

    void Update()
    {
        HandleLeanInput();

        if (Mathf.Abs(currentLeanAngle - targetLeanAngle) > leanThreshold)
        {
            currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLeanAngle, leanSpeed * Time.deltaTime);
        }
        else
        {
            currentLeanAngle = targetLeanAngle;
        }
    }

    void LateUpdate()
    {
        if (PlayerMesh == null) return;

        GetInput();
        float counterOffset = 0f;

        float leanNormalized = currentLeanAngle / maxLeanAngle;
        counterOffset = leanNormalized * maxCameraXOffset;
        transform.localPosition = new Vector3(StartLocalPos.x + counterOffset, StartLocalPos.y, StartLocalPos.z);
        
        Quaternion finalRotation = CalculateSwayRotation() * Quaternion.Euler(0f, 0f, currentLeanAngle);

        Vector3 bobPos = CalculateMovementBob();
        Vector3 finalPosition = new Vector3(bobPos.x , bobPos.y, bobPos.z);

        PlayerMesh.localRotation = Quaternion.Slerp(PlayerMesh.localRotation, finalRotation, swaySpeed * Time.deltaTime);
        PlayerMesh.localPosition = Vector3.Lerp(PlayerMesh.localPosition, finalPosition, Time.deltaTime * bobSpeed);
    }

    private void FixedUpdate()
    {
        CheckCanLean();
    }

    void GetInput()
    {
        mouseMovement.x = Mathf.Clamp(Input.GetAxisRaw("Mouse X") * swayAmount, -maxSway, maxSway);
        mouseMovement.y = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * swayAmount, -maxSway, maxSway);
        physicalMovement.x = Input.GetAxisRaw("Horizontal");
        physicalMovement.y = Input.GetAxisRaw("Vertical");
    }

    void HandleLeanInput()
    {
        if (IsRightBlocked && targetLeanAngle == -maxLeanAngle)
        {
            targetLeanAngle = 0f;
        }

        if (IsLeftBlocked && targetLeanAngle == maxLeanAngle)
        {
            targetLeanAngle = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Mathf.Approximately(targetLeanAngle, maxLeanAngle))
                targetLeanAngle = 0f; // Toggle off
            else if (!IsLeftBlocked)
                targetLeanAngle = maxLeanAngle; // Lean if clear
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Mathf.Approximately(targetLeanAngle, -maxLeanAngle))
                targetLeanAngle = 0f; // Toggle off
            else if (!IsRightBlocked)
                targetLeanAngle = -maxLeanAngle; // Lean if clear
        }
    }

    Quaternion CalculateSwayRotation()
    {
        return Quaternion.AngleAxis(-mouseMovement.y, Vector3.right) * Quaternion.AngleAxis(mouseMovement.x, Vector3.up);
    }

    Vector3 CalculateMovementBob()
    {
        float moveX = Mathf.Clamp(physicalMovement.x * moveSwayAmount, -maxMoveSway, maxMoveSway);
        float moveZ = Mathf.Clamp(physicalMovement.y * moveSwayAmount, -maxMoveSway, maxMoveSway);
        float verticalVel = PlayerComponents.Instance.Movement.CurrentVelocity.y;
        float moveY = Mathf.Clamp((verticalVel / playerMovementData.JumpForce) * moveSwayAmount * jumpBobMultiplier, -maxMoveSway, maxMoveSway);

        return meshStartLocalPos + new Vector3(-moveX * MoveMultipiler, moveY, -moveZ * MoveMultipiler);
    }


    void CheckCanLean()
    {
        IsRightBlocked = Physics.SphereCast(CheckOrigin.position, leanCheckRadius, CheckOrigin.right, out _, leanCheckDistance);

        IsLeftBlocked = Physics.SphereCast(CheckOrigin.position, leanCheckRadius, - CheckOrigin.right, out _, leanCheckDistance);
    }

    private void OnDrawGizmos()
    {
        // right
        Gizmos.color = IsRightBlocked ? Color.red : Color.green;
        Vector3 rightEndPos = CheckOrigin.position + (CheckOrigin.right * leanCheckDistance);
        Gizmos.DrawWireSphere(CheckOrigin.position, leanCheckRadius); 
        Gizmos.DrawLine(CheckOrigin.position, rightEndPos);           
        Gizmos.DrawWireSphere(rightEndPos, leanCheckRadius);        

        // Left Check Visualization
        Gizmos.color = IsLeftBlocked ? Color.red : Color.green;
        Vector3 leftEndPos = CheckOrigin.position + (-CheckOrigin.right * leanCheckDistance);
        Gizmos.DrawLine(CheckOrigin.position, leftEndPos);
        Gizmos.DrawWireSphere(leftEndPos, leanCheckRadius);
    }
}