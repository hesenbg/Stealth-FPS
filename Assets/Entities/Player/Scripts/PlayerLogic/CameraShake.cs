using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("Shake intensity for each axis (X, Y, Z)")]
    public Vector3 shakeIntensity = new Vector3(0.1f, 0.1f, 0.05f);

    [Tooltip("How long the shake lasts")]
    public float shakeDuration = 0.5f;

    [Tooltip("How quickly the shake diminishes")]
    public float shakeDecay = 1.5f;

    [Header("Runtime Info")]
    [SerializeField] private float currentShakeDuration = 0f;

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            if (currentShakeDuration > 0)
            {
                // Generate random offset based on shake intensity per axis
                Vector3 shakeOffset = new Vector3(
                    Random.Range(-shakeIntensity.x, shakeIntensity.x),
                    Random.Range(-shakeIntensity.y, shakeIntensity.y),
                    Random.Range(-shakeIntensity.z, shakeIntensity.z)
                );

                // Apply shake with diminishing intensity over time
                float decayFactor = currentShakeDuration / shakeDuration;
                transform.localPosition = originalPosition + shakeOffset * decayFactor;

                // Decrease shake duration
                currentShakeDuration -= Time.deltaTime * shakeDecay;
            }
            else
            {
                // Shake finished, return to original position
                StopShake();
            }
        }
    }

    /// Triggers a camera shake with default settings
    public void TriggerShake()
    {
        originalPosition = transform.localPosition;
        currentShakeDuration = shakeDuration;
        isShaking = true;
    }

    /// Triggers a camera shake with custom intensity and duration
    public void TriggerShake(Vector3 customIntensity, float customDuration)
    {
        shakeIntensity = customIntensity;
        shakeDuration = customDuration;
        TriggerShake();
    }

    /// Triggers a camera shake with custom intensity only
    public void TriggerShake(Vector3 customIntensity)
    {
        shakeIntensity = customIntensity;
        TriggerShake();
    }

    /// Stops the shake immediately and returns camera to original position
    public void StopShake()
    {
        isShaking = false;
        currentShakeDuration = 0f;
        transform.localPosition = originalPosition;
    }

    /// Check if camera is currently shaking
    public bool IsShaking()
    {
        return isShaking;
    }
}
