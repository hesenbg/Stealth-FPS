using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralADS : MonoBehaviour
{
    [SerializeField] Transform ADSposition;
    [SerializeField] Transform RightHand;
    [SerializeField] float Speed ;

    Vector3 originalLocalPos;

    [SerializeField] float ZoomField;
    float OriginalZoomField =60;

    [SerializeField] Rig ADSRig;

    ArmsPositionRigController ADSController;

    void Start()
    {
        OriginalZoomField = PlayerData.GetCamera().fieldOfView;
        PlayerData.SetADSrig(ADSRig);
        // initialize once
        originalLocalPos = RightHand.localPosition;

        ADSController = GetComponent<ArmsPositionRigController>();

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
            PlayerData.GetCamera().fieldOfView =
                Mathf.Lerp(PlayerData.GetCamera().fieldOfView, OriginalZoomField, Speed * Time.deltaTime);

            ADSRig.weight =
                Mathf.Lerp(ADSRig.weight, 0f, Speed * Time.deltaTime);

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalLocalPos,
                Speed * Time.deltaTime);
        }
        else
        {
            PlayerData.GetCamera().fieldOfView =
                Mathf.Lerp(PlayerData.GetCamera().fieldOfView, ZoomField, Speed * Time.deltaTime);

            ADSRig.weight =
                Mathf.Lerp(ADSRig.weight, 1f, Speed * Time.deltaTime);

            transform.position = Vector3.Lerp(
                transform.position,
                ADSposition.position,
                Speed * Time.deltaTime);
        }
    }


    void Update()
    {
        TakeInput();
        //UpdateADS();

        ADSController.MoveArms(Speed,ADSposition.position,aiming,1,0);

    }
}
