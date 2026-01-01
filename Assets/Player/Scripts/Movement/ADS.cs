using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ADS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform rightHand;
    [SerializeField] Transform adsPosition;
    [SerializeField] Rig adsRig;

    [Header("Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float maxWeight = 1f;
    [SerializeField] float minWeight = 0f;
    [SerializeField] float zoomFOV = 40f;

    Vector3 originalLocalPos;
    float originalFOV;

    void Awake()
    {
        originalLocalPos = rightHand.localPosition;
        originalFOV = PlayerData.GetCamera().fieldOfView;
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
            adsPosition.position,
            speed * Time.deltaTime
        );

        PlayerData.GetCamera().fieldOfView = Mathf.Lerp(
            PlayerData.GetCamera().fieldOfView,
            zoomFOV,
            speed * Time.deltaTime
        );
    }

    public void RevertADS()
    {
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

        PlayerData.GetCamera().fieldOfView = Mathf.Lerp(
            PlayerData.GetCamera().fieldOfView,
            originalFOV,
            speed * Time.deltaTime
        );
    }
}
