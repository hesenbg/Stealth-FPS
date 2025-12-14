using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ADS : MonoBehaviour
{
    [SerializeField] Transform ADSposition;
    [SerializeField] Transform RightHand;
    [SerializeField] float Speed ;

    Vector3 originalLocalPos;

    [SerializeField] float ZoomField;
    float OriginalZoomField =60;

    [SerializeField] Rig ADSRig;

    void Start()
    {
        OriginalZoomField = PlayerData.GetCamera().fieldOfView;
        PlayerData.SetADSrig(ADSRig);
        // initialize once
        originalLocalPos = RightHand.position;
    }
    [SerializeField] bool aiming;

    void TakeInput()
    {
        aiming = false;
        if (!PlayerData.GetAnimationLogic().canADS)
        {
            ADSRig.weight = Mathf.Lerp(ADSRig.weight, 0f, Speed * Time.deltaTime);
            return;
        }

        aiming = Input.GetMouseButton(1);
    }

    void UpdateADS()
    {
        if (!aiming)
        {
            // FOW
            PlayerData.GetCamera().fieldOfView = Mathf.Lerp(PlayerData.GetCamera().fieldOfView, OriginalZoomField, Time.deltaTime * Speed);

            // while NOT aiming  update the default position continuously
            originalLocalPos = transform.position;
            ADSRig.weight = 0;

            transform.position = Vector3.Lerp(
                transform.position,
                originalLocalPos,  // effectively stays where it is
                Speed * Time.deltaTime);
        }
        else
        {
            // FOW
            PlayerData.GetCamera().fieldOfView = Mathf.Lerp(PlayerData.GetCamera().fieldOfView, ZoomField, Time.deltaTime * Speed);

            // aiming  freeze original position
            ADSRig.weight = Mathf.Lerp(ADSRig.weight, 1, Speed * Time.deltaTime);

            transform.position = Vector3.Lerp(
                transform.position,
                ADSposition.position,
                Speed * Time.deltaTime);
        }
    }

    void Update()
    {
        TakeInput();
        UpdateADS();

    }
}
