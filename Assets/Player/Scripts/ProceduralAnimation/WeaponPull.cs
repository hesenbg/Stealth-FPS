using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponPull : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] float threshold = 1.0f;
    [SerializeField] LayerMask hitMask;

    [Header("Rig")]
    [SerializeField] Transform rightHand;
    [SerializeField] Rig weaponPullRig;
    [SerializeField] float weightSpeed = 6f;
    [SerializeField] Transform PullBackPosition;

    [Header("Tuning")]
    [SerializeField] float MaXDistance; 
    [SerializeField] float MaxWeight;
    [SerializeField] float MinWeight;

    public bool blocked;

    [SerializeField] float TresholdForMaxBlock;

    void Update()
    {
        blocked = Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit hit,
            threshold,
            hitMask
        );

        float dynamicMaxWeight = MinWeight;

        float distance =0;

        if (blocked)
        {
            distance = hit.distance;

            // 1 when very close, 0 when far
            float Ratio = 1-(distance / threshold);

            dynamicMaxWeight = Mathf.Lerp(MinWeight, MaxWeight*Ratio, weightSpeed);
        }

        Debug.Log(distance);

        PlayerData.GetArmRigLogic().MoveArms(
            weightSpeed,
            PullBackPosition.position,
            blocked,
            dynamicMaxWeight,
            MinWeight
        );
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + transform.forward * threshold
        );
    }
}