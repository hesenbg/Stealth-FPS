using UnityEngine;
using UnityEngine.Animations.Rigging;
public class ADS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform RigParent;
    [SerializeField] Rig GunRig;
    [SerializeField] Transform ADSpos;

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
    }

    public bool ApplyADS()
    {
        GunRig.weight = Mathf.Lerp(
            GunRig.weight,
            maxWeight,
            speed * Time.deltaTime
        );

        RigParent.position = Vector3.Lerp(
            RigParent.position,
            ADSpos.transform.position,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            zoomFOV,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(RigParent.position, ADSpos.position) < 0.01f)
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
        if (GunRig.weight <= minWeight + 0.001f) return;

        GunRig.weight = Mathf.Lerp(
            GunRig.weight,
            minWeight,
            speed * Time.deltaTime
        );

        RigParent.localPosition = Vector3.Lerp(
            RigParent.localPosition,
            originalLocalPos,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            originalFOV,
            speed * Time.deltaTime
        );

        if (GunRig.weight <= minWeight + 0.001f)
        {
            GunRig.weight = minWeight;
            RigParent.localPosition = originalLocalPos;
            PlayerComponents.Instance.MainCamera.fieldOfView = originalFOV;
        }
    }
}