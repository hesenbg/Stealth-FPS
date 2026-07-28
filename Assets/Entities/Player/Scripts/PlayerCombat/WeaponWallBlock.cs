using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponWallBlock : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] float maxDistance = 1.0f;
    [SerializeField] float sphereRadius = 0.15f;
    [SerializeField] Transform CastSource;
    [SerializeField] Vector3 PosOffset;

    [Header("Rig & Smoothing")]
    [SerializeField] Rig weaponPullRig;
    [SerializeField] float weightSpeed = 8f;
    [SerializeField] float MinWeightBlock; // minimum weight for consider it to be 

    [Header("Procedural Transformation")]
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 maxPullbackOffset;
    [SerializeField] Vector3 maxRotationOffset;

    [Header("Blocking Logic")]
    [SerializeField] float BlockTresholdLimit = 0.1f;
    public bool Blocked;
    [SerializeField] LayerMask BlockAble;

    [SerializeField] private float currentWeight;
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    private RaycastHit lastHit;
    private bool wasBlocked;

    // events

    public event EventHandler WallBlockStart;
    public event EventHandler WallBlockEnd;

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
        //Detection
        wasBlocked = Physics.SphereCast(
            CastSource.position+PosOffset,
            sphereRadius,
            CastSource.forward,
            out lastHit,
            maxDistance
            ,BlockAble
            ,QueryTriggerInteraction.Ignore
        );

        //Weight Calculation
        float targetWeight = 0f;
        if (wasBlocked)
        {
            float distanceRatio = lastHit.distance / maxDistance;
            targetWeight = Mathf.Clamp01(1f - distanceRatio);
        }

        currentWeight = Mathf.Lerp(currentWeight, targetWeight, weightSpeed * Time.deltaTime);

        // If the weight is practically zero, reset to initial and stop updating transform
        if (currentWeight > MinWeightBlock)
        {
            weaponPullRig.weight = currentWeight;
            ApplyOffsets(currentWeight);

            // Set Blocked status based on threshold
            Blocked = currentWeight > BlockTresholdLimit;
        }
        else if (currentWeight <= MinWeightBlock && weaponPullRig.weight > 0f)
        {
            // Final reset to ensure clean values
            currentWeight = 0f;
            weaponPullRig.weight = 0f;
            ResetOffsets();
            Blocked = false;
        }

        FireEvents();
    }

    void ApplyOffsets(float weight)
    {
        if (targetTransform == null) return;

        targetTransform.localPosition = initialLocalPos + (maxPullbackOffset * weight);
        targetTransform.localRotation = initialLocalRot * Quaternion.Euler(maxRotationOffset * weight);
    }

    void ResetOffsets()
    {
        if (targetTransform == null) return;
        targetTransform.localPosition = initialLocalPos;
        targetTransform.localRotation = initialLocalRot;
    }
    bool prevBlocked;
    void FireEvents()
    {
        if (Blocked == prevBlocked)
            return;

        if (Blocked)
            WallBlockStart?.Invoke(this, EventArgs.Empty);
        else
            WallBlockEnd?.Invoke(this, EventArgs.Empty);

        prevBlocked = Blocked;
    }


    private void OnDrawGizmos()
    {
        Vector3 pos = CastSource.position + PosOffset;

        Gizmos.color = Color.cyan;
        Vector3 endPoint = pos + CastSource.forward * maxDistance;
        Gizmos.DrawLine(pos, endPoint);
        Gizmos.DrawWireSphere(endPoint, sphereRadius);

        if (Application.isPlaying && wasBlocked)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHit.point, 0.05f);

            Vector3 sphereCenterAtHit = pos + CastSource.forward * lastHit.distance;
            Gizmos.DrawWireSphere(sphereCenterAtHit, sphereRadius);     
        }
    }
}