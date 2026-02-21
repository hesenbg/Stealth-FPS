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

    void GetInput()
    {
        mouseMovement.x = Mathf.Clamp(Input.GetAxisRaw("Mouse X") * swayAmount, -maxSway, maxSway);
        mouseMovement.y = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * swayAmount, -maxSway, maxSway);
        physicalMovement.x = Input.GetAxisRaw("Horizontal");
        physicalMovement.y = Input.GetAxisRaw("Vertical");
    }

    void HandleLeanInput()
    {
        if (currentLeanMode == LeanMode.Hold)
        {
            targetLeanAngle = 0f;
            if (Input.GetKey(KeyCode.Q)) targetLeanAngle = maxLeanAngle;
            else if (Input.GetKey(KeyCode.E)) targetLeanAngle = -maxLeanAngle;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
                targetLeanAngle = Mathf.Approximately(targetLeanAngle, maxLeanAngle) ? 0f : maxLeanAngle;
            if (Input.GetKeyDown(KeyCode.E))
                targetLeanAngle = Mathf.Approximately(targetLeanAngle, -maxLeanAngle) ? 0f : -maxLeanAngle;
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
}