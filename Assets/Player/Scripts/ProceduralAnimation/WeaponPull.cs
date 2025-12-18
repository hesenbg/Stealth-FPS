using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponPull : MonoBehaviour
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
    [SerializeField] Vector3 maxPullAngle ;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float maxPullDistance = 1.0f;
    [SerializeField] float minPullDistance = 0.2f;
    [SerializeField] float returnToleranceDegrees = 1.0f;

    Quaternion rightDefaultRotation;
    Quaternion leftDefaultRotation;

    public bool blocked;

    [SerializeField] ArmRotationRigController Pull;

    void Awake()
    {
        PlayerData.SetPlayerPullLogiv(this);

        rightDefaultRotation = rightHand.localRotation;
        leftDefaultRotation = leftHand.localRotation;
    }

    void UpdatePull(RaycastHit hit)
    {
        Quaternion rightTarget = rightDefaultRotation;
        Quaternion leftTarget = leftDefaultRotation;

        if (blocked)
        {
            float distance = Vector3.Distance(transform.position, hit.point);

            float t = (maxPullDistance - distance) /
                      (maxPullDistance - minPullDistance);

            t = Mathf.Clamp01(t);

            Vector3 pullAngle = t * maxPullAngle;

            rightTarget = Quaternion.Euler(pullAngle) * rightDefaultRotation;
            leftTarget = Quaternion.Euler(pullAngle) * leftDefaultRotation;
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
            weaponPullRig.weight = Mathf.Lerp(weaponPullRig.weight, MaxWeight, Time.deltaTime * WeightSpeed);
        }
        else
        {
            float rightAngle = Quaternion.Angle(rightHand.localRotation, rightDefaultRotation);
            float leftAngle = Quaternion.Angle(leftHand.localRotation, leftDefaultRotation);

            bool handsReturned =
                rightAngle <= returnToleranceDegrees &&
                leftAngle <= returnToleranceDegrees;

            TargetRigWeight = handsReturned ? MinWeight : MaxWeight;

            weaponPullRig.weight = Mathf.Lerp(weaponPullRig.weight, TargetRigWeight, Time.deltaTime * WeightSpeed);
        }
    }

    void Update()
    {
        blocked = Physics.Raycast(
            Source.position,
            Source.forward,
            out RaycastHit hit,
            threshold,
            hitMask
        );

        //UpdatePull(hit);

        Pull.RotateArms(blocked, maxPullAngle, rotationSpeed, 1, 0);
    }

    [SerializeField] float MinWeight;
    [SerializeField] float MaxWeight;
}