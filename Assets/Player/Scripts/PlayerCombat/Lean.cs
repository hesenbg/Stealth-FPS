using UnityEngine;

public class Lean : MonoBehaviour
{
    [Header("Lean Settings")]
    [SerializeField] float maxLeanAngle = 15f;
    [SerializeField] float maxCameraOffset = 0.25f;
    [SerializeField] float speed = 8f;
    [SerializeField] float threshold = 0.01f; 
    [Header("References")]
    [SerializeField] Transform bodyTransform;
    [SerializeField] Transform cameraTransform;

    float currentAngle;
    float targetAngle;
    bool isMoving;

    Vector3 camInitialLocalPos;
    Quaternion bodyInitialLocalRot;

    enum LeanMode { Toggle, Hold }
    [SerializeField] LeanMode CurrentLeanMode;

    void Start()
    {
        camInitialLocalPos = cameraTransform.localPosition;
        bodyInitialLocalRot = bodyTransform.localRotation;
    }

    void Update()
    {
        float previousTarget = targetAngle;
        HandleInput();

        if (previousTarget != targetAngle)
        {
            isMoving = true;
        }

        if (isMoving)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, speed * Time.deltaTime);

            // Check if we are within the 0.01 threshold
            if (Mathf.Abs(currentAngle - targetAngle) < threshold)
            {
                currentAngle = targetAngle;
                ApplyTransformations(); // Final snap to exact position
                isMoving = false;       // Stop updating until input changes
            }
        }
    }

    void HandleInput()
    {
        if (CurrentLeanMode == LeanMode.Hold)
        {
            targetAngle = 0f;
            if (Input.GetKey(KeyCode.Q)) targetAngle = maxLeanAngle;
            else if (Input.GetKey(KeyCode.E)) targetAngle = -maxLeanAngle;
        }
        else // Toggle Mode
        {
            if (Input.GetKeyDown(KeyCode.Q))
                targetAngle = Mathf.Approximately(targetAngle, maxLeanAngle) ? 0f : maxLeanAngle;

            if (Input.GetKeyDown(KeyCode.E))
                targetAngle = Mathf.Approximately(targetAngle, -maxLeanAngle) ? 0f : -maxLeanAngle;
        }
    }

    void LateUpdate()
    {
        // Only run the heavy transform updates if the flag is true
        if (isMoving)
        {
            ApplyTransformations();
        }
    }

    void ApplyTransformations()
    {
        // Body Rotation
        bodyTransform.localRotation = bodyInitialLocalRot * Quaternion.Euler(0f, 0f, currentAngle);

        // Camera Offset
        float normalized = currentAngle / maxLeanAngle;
        Vector3 targetLocalPos = camInitialLocalPos + (Vector3.right * (normalized * maxCameraOffset));
        cameraTransform.localPosition = targetLocalPos;
    }
}