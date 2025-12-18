using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmRotationRigController : MonoBehaviour
{
    void Start()
    {
        RightHandOriginalRotation = RightHandJoint.eulerAngles;
    }

    [SerializeField] Transform RightHandJoint;
    [SerializeField] Transform LeftHandJoint;

    [SerializeField] Rig RotationRig;

    Vector3 RightHandOriginalRotation;
    Vector3 LeftHandOriginalRotation;

    public void RotateArms(bool IsActive, Vector3 EulerRotation, float Speed, float MaxWeight, float MinWeight)
    {
        if (IsActive)
        {
            RotationRig.weight = Mathf.Lerp(RotationRig.weight,MaxWeight, Speed*Time.deltaTime);

            RightHandJoint.Rotate(Vector3.Lerp(RightHandJoint.eulerAngles,EulerRotation,Speed*Time.deltaTime));
        }
        else
        {
            RotationRig.weight = Mathf.Lerp(RotationRig.weight,MinWeight, Speed*Time.deltaTime);
            RightHandJoint.Rotate(Vector3.Lerp(RightHandJoint.eulerAngles,RightHandOriginalRotation,Speed*Time.deltaTime));
        }

    }
}
