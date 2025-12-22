using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmsPositionRigController : MonoBehaviour
{
    [SerializeField] Transform RightHandJoint;
    [SerializeField] Transform LeftHandJoint;

    [SerializeField] Rig PositionRig;

    Vector3 RightHandOriginalPosition;

    void Start()
    {
        RightHandOriginalPosition = RightHandJoint.localPosition;
        PlayerData.SetArmRigLogic(this);
    }

    public void MoveArms(float Speed, Vector3 Position, bool IsActive, float MaxWeight, float MinWeight)
    {
        if (IsActive)
        {
            // adjust weight
            PositionRig.weight = Mathf.Lerp(PositionRig.weight, MaxWeight, Speed*Time.deltaTime);

            // interpolate the position
            RightHandJoint.position = Vector3.Lerp(RightHandJoint.position, Position, Speed*Time.deltaTime);
        }
        else
        {
            // adjust weight
            PositionRig.weight = Mathf.Lerp(PositionRig.weight, MinWeight, Speed * Time.deltaTime);

            // interpolate the position
            RightHandJoint.localPosition = Vector3.Lerp(RightHandJoint.localPosition, RightHandOriginalPosition, Speed * Time.deltaTime);
        }
    }

    public void MoveArms()
    {

    }
}