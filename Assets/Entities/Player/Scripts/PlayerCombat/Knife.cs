using TMPro;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] Transform Origin;

    [Header("Hitbox args")]
    [SerializeField] float Radius = 0.3f;
    [SerializeField] float Distance = 1.0f;

    [SerializeField] bool ShowGizmos;
    [SerializeField] LayerMask Enemy;
    [SerializeField] TextMeshProUGUI AttackIndicator;

    private void FixedUpdate()
    {
        if (Physics.Raycast(Origin.position, Origin.forward, Distance+Radius,Enemy))
        {
            AttackIndicator.enabled = true;
        }
        else
        {
            AttackIndicator.enabled = false;
        }
    }

    public void Damage()
    {
        RaycastHit HitInfo;

        if (Physics.SphereCast(Origin.position, Radius, Origin.forward, out HitInfo, Distance))
        {
            if (HitInfo.collider.CompareTag("Head") || HitInfo.collider.CompareTag("Body"))
            {
                HitInfo.collider.gameObject.GetComponentInParent<HealthManager>().GetKnifeDamage();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (ShowGizmos == false) return;

        Gizmos.color = Color.red;

        Vector3 start = Origin.position;
        Vector3 end = start + Origin.forward * Distance;

        // Start sphere
        Gizmos.DrawWireSphere(start, Radius);

        // End sphere
        Gizmos.DrawWireSphere(end, Radius);

        // Lines showing the cast path
        Gizmos.DrawLine(start , end );
    }
}
