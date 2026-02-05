using UnityEngine;

public class Lean : MonoBehaviour
{
    [Header("Lean Settings")]
    [SerializeField] float maxLeanAngle = 15f;
    [SerializeField] float maxCameraOffset = 0.25f;
    [SerializeField] float speed = 8f;

    [Header("References")]
    [SerializeField] Transform bodyTransform;
    [SerializeField] Transform cameraTransform;

    float currentAngle;
    float targetAngle;

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
        HandleInput();

        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            speed * Time.deltaTime
        );
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
                targetAngle = (targetAngle == maxLeanAngle) ? 0f : maxLeanAngle;

            if (Input.GetKeyDown(KeyCode.E))
                targetAngle = (targetAngle == -maxLeanAngle) ? 0f : -maxLeanAngle;
        }
    }

    void LateUpdate()
    {
        ApplyBodyLean();
        ApplyCameraOffset();
    }

    void ApplyBodyLean()
    {
        bodyTransform.localRotation =
            bodyInitialLocalRot * Quaternion.Euler(0f, 0f, currentAngle);
    }

    void ApplyCameraOffset()
    {
        float normalized = currentAngle / maxLeanAngle;
        Vector3 targetLocalPos = camInitialLocalPos + (Vector3.right * (normalized * maxCameraOffset));

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetLocalPos,
            speed * Time.deltaTime
        );
    }
}