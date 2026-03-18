using UnityEngine;
using UnityEngine.Animations.Rigging;
public class ADS : MonoBehaviour
{
    [SerializeField] Transform ADSpos;
    [SerializeField] Transform RigParent;

    float Weight;
    Vector3 Position;

    [Header("Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float maxWeight = 1f;
    [SerializeField] float minWeight = 0f;
    [SerializeField] float zoomFOV = 40f;

    Vector3 originalLocalPos;
    float originalFOV;

    void Start()
    {
        originalLocalPos = RigParent.localPosition;
        originalFOV = PlayerComponents.Instance.MainCamera.fieldOfView;
        Position = originalLocalPos; 
    }

    public bool ApplyADS()
    {
        Weight = Mathf.Lerp(
            Weight,
            maxWeight,
            speed * Time.deltaTime
        );

        Position = Vector3.Lerp(
            Position,
            ADSpos.transform.localPosition,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            zoomFOV,
            speed * Time.deltaTime
        );

        GunRigController.Instance.ApplyPosWeigth(Position, Weight);

        if (Vector3.Distance(Position, ADSpos.localPosition) < 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RevertADS() // revert ads doesnt set values after reaching a certain treshold
    {
        if (Weight <= minWeight + 0.001f) return;

        Weight = Mathf.Lerp(
            Weight,
            minWeight,
            speed * Time.deltaTime
        );

        Position = Vector3.Lerp(
            Position,
            originalLocalPos,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            originalFOV,
            speed * Time.deltaTime
        );

        GunRigController.Instance.ApplyPosWeigth(Position,Weight);

        if (Weight <= minWeight + 0.001f)
        {
            Weight = minWeight;
            Position = originalLocalPos;
            PlayerComponents.Instance.MainCamera.fieldOfView = originalFOV;
        }
    }


    public void ResetADS()
    {
        Weight = minWeight;
        Position = originalLocalPos;
        PlayerComponents.Instance.MainCamera.fieldOfView = originalFOV;
        GunRigController.Instance.ApplyPosWeigth(Position, Weight);
    }
}