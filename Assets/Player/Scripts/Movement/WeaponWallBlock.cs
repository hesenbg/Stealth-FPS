using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponWallBlock : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] LayerMask hitMask;
    [SerializeField] float maxDistance = 1.0f;
    [SerializeField] float sphereRadius = 0.15f;

    [Header("Rig & Smoothing")]
    [SerializeField] Rig weaponPullRig;
    [SerializeField] float weightSpeed = 8f;

    [Header("Procedural Transformation")]
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 maxPullbackOffset;
    [SerializeField] Vector3 maxRotationOffset;

    private float currentWeight;
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    private RaycastHit lastHit;
    private bool wasBlocked;

    [SerializeField] float BlockTresholdLimit;

    public bool Blocked {  get; private set; }

    void Start()
    {
        if (targetTransform != null)
        {
            initialLocalPos = targetTransform.localPosition;
            initialLocalRot = targetTransform.localRotation;
        }
    }

    void Update()
    {
        if(currentWeight <BlockTresholdLimit)
        {
            Blocked = true;
        }
        else
        {
            Blocked = false;
        }

        wasBlocked = Physics.SphereCast(
            transform.position,
            sphereRadius,
            transform.forward,
            out lastHit,
            maxDistance,
            hitMask
        );

        float targetWeight = 0f;

        if (wasBlocked)
        {
            float distanceRatio = lastHit.distance / maxDistance;
            targetWeight = Mathf.Clamp01(1f - distanceRatio);
        }

        currentWeight = Mathf.Lerp(currentWeight, targetWeight, weightSpeed * Time.deltaTime);

        weaponPullRig.weight = currentWeight;
        ApplyOffsets(currentWeight);
    }

    void ApplyOffsets(float weight)
    {
        if (targetTransform == null) return;

        targetTransform.localPosition = initialLocalPos + (maxPullbackOffset * weight);
        targetTransform.localRotation = initialLocalRot * Quaternion.Euler(maxRotationOffset * weight);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 endPoint = transform.position + transform.forward * maxDistance;
        Gizmos.DrawLine(transform.position, endPoint);
        Gizmos.DrawWireSphere(endPoint, sphereRadius);

        if (Application.isPlaying && wasBlocked)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHit.point, 0.05f);

            Vector3 sphereCenterAtHit = transform.position + transform.forward * lastHit.distance;
            Gizmos.DrawWireSphere(sphereCenterAtHit, sphereRadius);
        }
    }
}