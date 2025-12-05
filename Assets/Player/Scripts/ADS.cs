using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ADS : MonoBehaviour
{
    [SerializeField] Transform ADSposition;
    [SerializeField] float Speed ;

    Vector3 originalLocalPos;

    [SerializeField] Rig ADSRig;

    void Start()
    {
        PlayerData.SetADSrig(ADSRig);
        // initialize once
        originalLocalPos = transform.position;
    }

    void Update()
    {


        if (!PlayerData.GetAnimationLogic().canADS)
        {
            ADSRig.weight = 0f;
            return;
        }

        bool aiming = Input.GetMouseButton(1);

        if (!aiming)
        {
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
            // aiming  freeze original position
            ADSRig.weight = Mathf.Lerp(ADSRig.weight, 1, Speed*Time.deltaTime);

            transform.position = Vector3.Lerp(
                transform.position,
                ADSposition.position,
                Speed * Time.deltaTime);
        }
    }
}
