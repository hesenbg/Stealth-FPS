using UnityEngine;
using UnityEngine.Animations.Rigging;
public class ADS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform rightHand;
    [SerializeField] Rig adsRig;
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
        originalLocalPos = rightHand.localPosition;
        originalFOV = PlayerComponents.Instance.MainCamera.fieldOfView;
    }

    public void ApplyADS()
    {
        adsRig.weight = Mathf.Lerp(
            adsRig.weight,
            maxWeight,
            speed * Time.deltaTime
        );

        rightHand.position = Vector3.Lerp(
            rightHand.position,
            ADSpos.transform.position,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            zoomFOV,
            speed * Time.deltaTime
        );
    }

    public void RevertADS() // revert ads doesnt set values after reaching a certain treshold
    {
        if (adsRig.weight <= minWeight + 0.001f) return;

        adsRig.weight = Mathf.Lerp(
            adsRig.weight,
            minWeight,
            speed * Time.deltaTime
        );

        rightHand.localPosition = Vector3.Lerp(
            rightHand.localPosition,
            originalLocalPos,
            speed * Time.deltaTime
        );

        PlayerComponents.Instance.MainCamera.fieldOfView = Mathf.Lerp(
            PlayerComponents.Instance.MainCamera.fieldOfView,
            originalFOV,
            speed * Time.deltaTime
        );

        if (adsRig.weight <= minWeight + 0.001f)
        {
            adsRig.weight = minWeight;
            rightHand.localPosition = originalLocalPos;
            PlayerComponents.Instance.MainCamera.fieldOfView = originalFOV;
        }
    }
}