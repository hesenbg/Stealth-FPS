using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponPullLogic : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] float threshold = 1.0f;
    [SerializeField] LayerMask hitMask;
    [SerializeField] Transform Source;

    [Header("Rig")]
    [SerializeField] Rig weaponPullRig;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;
    float TargetRigWeight;
    [SerializeField] float WeightSpeed;

    [Header("Tuning")]
    [SerializeField] float maxPullAngle = 25f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float maxPullDistance = 1.0f;
    [SerializeField] float minPullDistance = 0.2f;
    [SerializeField] float returnToleranceDegrees = 1.0f;

    Quaternion rightDefaultRotation;
    Quaternion leftDefaultRotation;

    void Awake()
    {
        rightDefaultRotation = rightHand.localRotation;
        leftDefaultRotation = leftHand.localRotation;
    }

    void Update()
    {
        bool blocked = Physics.Raycast(
            Source.position,
            Source.forward,
            out RaycastHit hit,
            threshold,
            hitMask
        );

        Quaternion rightTarget = rightDefaultRotation;
        Quaternion leftTarget = leftDefaultRotation;

        if (blocked)
        {
            float distance = Vector3.Distance(transform.position, hit.point);

            float t = (maxPullDistance - distance) /
                      (maxPullDistance - minPullDistance);

            t = Mathf.Clamp01(t);

            float pullAngle = t * maxPullAngle;

            rightTarget = Quaternion.Euler(-pullAngle, 0f, 0f) * rightDefaultRotation;
            leftTarget = Quaternion.Euler(-pullAngle, 0f, 0f) * leftDefaultRotation;
        }

        // Rotate hands first
        rightHand.localRotation = Quaternion.Slerp(
            rightHand.localRotation,
            rightTarget,
            Time.deltaTime * rotationSpeed
        );

        leftHand.localRotation = Quaternion.Slerp(
            leftHand.localRotation,
            leftTarget,
            Time.deltaTime * rotationSpeed
        );

        // Decide rig weight AFTER rotation update
        if (blocked)
        {
            weaponPullRig.weight = Mathf.Lerp(weaponPullRig.weight,1f, Time.deltaTime*WeightSpeed);
        }
        else
        {
            float rightAngle = Quaternion.Angle(rightHand.localRotation, rightDefaultRotation);
            float leftAngle = Quaternion.Angle(leftHand.localRotation, leftDefaultRotation);

            bool handsReturned =
                rightAngle <= returnToleranceDegrees &&
                leftAngle <= returnToleranceDegrees;

            TargetRigWeight = handsReturned ? 0f : 1f;

            weaponPullRig.weight = Mathf.Lerp(weaponPullRig.weight, TargetRigWeight, Time.deltaTime * WeightSpeed);

        }
    }

}
