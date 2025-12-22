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

    ArmsPositionRigController ADSController;

    [SerializeField] bool aiming;

    void Start()
    {
        OriginalZoomField = PlayerData.GetCamera().fieldOfView;
        PlayerData.SetADSrig(ADSRig);
        // initialize once
        originalLocalPos = RightHand.localPosition;

        ADSController = GetComponent<ArmsPositionRigController>();
    }

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

    void Update()
    {
        TakeInput();

        ADSController.MoveArms(Speed,ADSposition.position,aiming,1,0);
    }
}
