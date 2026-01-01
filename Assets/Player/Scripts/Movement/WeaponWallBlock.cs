using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations.Rigging;

// 
public class WeaponWallBlock : MonoBehaviour
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
    [SerializeField] float Radius;

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

        blocked = Physics.SphereCast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)), Radius, out var hitinfo, MaXDistance);

        float dynamicMaxWeight = MinWeight;

        float distance =0;

        if (blocked)
        {
            distance = hit.distance;

            // 1 when very close, 0 when far
            float Ratio = 1-(distance / threshold);

            dynamicMaxWeight = Mathf.Lerp(MinWeight, MaxWeight*Ratio, weightSpeed);
        }

        weaponPullRig.weight = dynamicMaxWeight;

        Debug.Log(distance);
    }

    void MoveArm()
    {
       
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