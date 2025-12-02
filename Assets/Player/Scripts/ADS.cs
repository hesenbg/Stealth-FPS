using UnityEngine;

public class ADS : MonoBehaviour
{
    [SerializeField] Transform ADSposition;

    [SerializeField] float Speed = 10f;

    // Store VALUES, not the transform
    Vector3 originalLocalPos;
    Quaternion originalLocalRot;

    void Start()
    {
        originalLocalPos = transform.localPosition;
        originalLocalRot = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            PlayerData.GetAnimationLogic().PlayerAnimator.enabled = false;
            // move toward ADS
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                ADSposition.localPosition,
                Speed * Time.deltaTime);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                ADSposition.localRotation,
                Speed * Time.deltaTime);
        }
        else
        {
            PlayerData.GetAnimationLogic().PlayerAnimator.enabled = true;
            // return to original saved values
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalLocalPos,
                Speed * Time.deltaTime);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                originalLocalRot,
                Speed * Time.deltaTime);
        }
    }
}
