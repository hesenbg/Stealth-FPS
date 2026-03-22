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

    [SerializeField] AnimationCurve InterpolationCurve;

    Vector3 originalLocalPos;
    float originalFOV;

    void Start()
    {
        originalLocalPos = RigParent.localPosition;
        originalFOV = PlayerComponents.Instance.MainCamera.fieldOfView;
        Position = originalLocalPos;
        Weight = minWeight;
    }

    public bool ApplyADS()
    {
        Weight = Mathf.MoveTowards(Weight, maxWeight, speed * Time.deltaTime);

        //Position = RigParent.localPosition+InterpolationCurve.Evaluate(Weight)*(ADSpos.localPosition- RigParent.localPosition);

        Position = Vector3.Lerp(
            Position,
            ADSpos.localPosition,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            zoomFOV,
            speed * Time.deltaTime
        );

        float distance = Vector3.Distance(Position, ADSpos.localPosition);
        float fovDiff = Mathf.Abs(PlayerComponents.Instance.MainCamera.fieldOfView - zoomFOV);

        if (distance < 0.1f && fovDiff < 0.1f)
        {
            Position = ADSpos.localPosition;
            PlayerComponents.Instance.MainCamera.fieldOfView = zoomFOV;
            GunRigController.Instance.ApplyPosWeigth(Position, Weight);
            return true;
        }

        GunRigController.Instance.ApplyPosWeigth(Position, Weight);
        return false;
    }

    public bool RevertADS()
    {
        if (Vector3.Distance(Position, originalLocalPos) < 0.001f && Weight == minWeight)
            return true;

        Position = Vector3.Lerp(Position, originalLocalPos, speed * Time.deltaTime);

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView, originalFOV, speed * Time.deltaTime);

        Weight = Mathf.Lerp(Weight, minWeight, speed * Time.deltaTime);

        if (Vector3.Distance(Position, originalLocalPos) < 0.01f &&
            Mathf.Abs(PlayerComponents.Instance.MainCamera.fieldOfView - originalFOV) < 0.1f)
        {
            Position = originalLocalPos;

            PlayerComponents.Instance.MainCamera.fieldOfView = originalFOV;

            Weight = minWeight;

            GunRigController.Instance.ApplyPosWeigth(Position, Weight);

            return true;
        }

        GunRigController.Instance.ApplyPosWeigth(Position, Weight);
        return false;
    }

    public void ResetADS()
    {
        Weight = minWeight;
        Position = originalLocalPos;
        PlayerComponents.Instance.MainCamera.fieldOfView = originalFOV;
        GunRigController.Instance.ApplyPosWeigth(Position, Weight);
    }
}