using UnityEngine;

public class Lean : MonoBehaviour
{
    [Header("Lean Settings")]
    [SerializeField] float maxLeanAngle = 15f;
    [SerializeField] float maxCameraOffset = 0.25f;
    [SerializeField] float speed = 8f;

    [Header("References")]
    [SerializeField] Transform bodyTransform;   // visual mesh root
    [SerializeField] Transform cameraTransform; // FPS camera

    float currentAngle;
    float targetAngle;

    Vector3 camInitialLocalPos;
    Quaternion bodyInitialLocalRot;

    void Start()
    {
        camInitialLocalPos = cameraTransform.localPosition;
        bodyInitialLocalRot = bodyTransform.localRotation;
    }

    void Update()
    {
        targetAngle = 0f;

        if (Input.GetKey(KeyCode.Q))
            targetAngle = maxLeanAngle;
        else if (Input.GetKey(KeyCode.E))
            targetAngle = -maxLeanAngle;

        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            speed * Time.deltaTime
        );
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

        Vector3 targetLocalPos = camInitialLocalPos;
        targetLocalPos += Vector3.right * (normalized * maxCameraOffset);

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetLocalPos,
            speed * Time.deltaTime
        );
    }
}
