using UnityEngine;

public class ProceduralArms : MonoBehaviour
{
    [Header("Mouse Sway (Arms)")]
    [SerializeField] float swayAmount = 1.5f;
    [SerializeField] float swaySpeed = 8f;
    [SerializeField] float maxSway = 2f;

    [Header("Movement Bobbing (Arms)")]
    [SerializeField] float moveSwayAmount = 0.05f;
    [SerializeField] float maxMoveSway = 0.1f;
    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float jumpBobMultiplier = 2f;

    [Header("Lean Settings")]
    Transform cameraTransform;
    [SerializeField] float maxLeanAngle = 15f;
    [SerializeField] float maxCameraXOffset = 0.25f; // Moves camera on X
    [SerializeField] float leanSpeed = 8f;
    [SerializeField] float leanThreshold = 0.01f;

    public enum LeanMode { Toggle, Hold }
    [SerializeField] LeanMode currentLeanMode;

    [Header("References")]
    [SerializeField] MovementData playerMovementData;

    // Internal State
    Vector3 armStartLocalPos;
    Vector3 camStartLocalPos;
    Vector2 mouseMovement;
    Vector2 physicalMovement;

    float currentLeanAngle;
    float targetLeanAngle;

    void Start()
    {
        cameraTransform = PlayerComponents.Instance.MainCamera.transform;
        armStartLocalPos = transform.localPosition;
        if (cameraTransform != null) camStartLocalPos = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleLeanInput();

        // Lerp the lean angle independently
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
        GetInput();

        // 1. CALCULATE CAMERA LEAN (X-Axis Offset)
        if (cameraTransform != null)
        {
            float leanNormalized = currentLeanAngle / maxLeanAngle;
            Vector3 targetCamPos = camStartLocalPos + (Vector3.right * (leanNormalized * maxCameraXOffset));
            cameraTransform.localPosition = targetCamPos;
        }

        // Combine mouse sway with the z-axis tilt from leaning
        Quaternion swayRot = CalculateSwayRotation();
        Quaternion leanRot = Quaternion.Euler(0f, 0f, currentLeanAngle);
        Quaternion finalRotation = swayRot * leanRot;

        // CALCULATE ARM BOBBING (WASD + Jump)
        Vector3 targetArmBobPos = CalculateMovementBob();

        // APPLY TO ARMS
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation, swaySpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetArmBobPos, Time.deltaTime * bobSpeed);
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
        else // Toggle
        {
            if (Input.GetKeyDown(KeyCode.Q))
                targetLeanAngle = Mathf.Approximately(targetLeanAngle, maxLeanAngle) ? 0f : maxLeanAngle;
            if (Input.GetKeyDown(KeyCode.E))
                targetLeanAngle = Mathf.Approximately(targetLeanAngle, -maxLeanAngle) ? 0f : -maxLeanAngle;
        }
    }

    Quaternion CalculateSwayRotation()
    {
        Quaternion rotX = Quaternion.AngleAxis(-mouseMovement.y, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(mouseMovement.x, Vector3.up);
        return rotX * rotY;
    }

    Vector3 CalculateMovementBob()
    {
        float moveX = Mathf.Clamp(physicalMovement.x * moveSwayAmount, -maxMoveSway, maxMoveSway);
        float moveZ = Mathf.Clamp(physicalMovement.y * moveSwayAmount, -maxMoveSway, maxMoveSway);

        float verticalVel = PlayerComponents.Instance.Movement.CurrentVelocity.y;
        float moveY = Mathf.Clamp((verticalVel / playerMovementData.JumpForce) * moveSwayAmount * jumpBobMultiplier, -maxMoveSway, maxMoveSway);

        return armStartLocalPos + new Vector3(-moveX, moveY, -moveZ);
    }
}