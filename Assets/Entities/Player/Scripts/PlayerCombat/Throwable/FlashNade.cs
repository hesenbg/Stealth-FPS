using System;
using UnityEngine;
public class FlashNade : BaseNade
{
    [SerializeField] LayerMask EffectMask;

    Collider[] Effecteds;

    public override void Init()
    {
        Effecteds = null;
    }
    public override void OnNadeActivated(object sender, EventArgs e)
    {
        ApplyEffect();
        Destroy(gameObject);
    }

    void ApplyEffect()
    {
        Effecteds = Physics.OverlapSphere(transform.position, EffectRadius, EffectMask, QueryTriggerInteraction.Ignore);

        foreach (Collider col in Effecteds)
        {
            Physics.Raycast(transform.position, (col.transform.position - transform.position).normalized, out RaycastHit hit);

            HealthManager hm = hit.collider.GetComponent<HealthManager>();

            if(hm != null)
            {
                hm.ApplyFlashEffect(NadeEffectDuration,(transform.position- col.transform.position).normalized);
            }
        }
    }

    public override void OnNadeDeactivated(object sender, EventArgs e)
    {

    }

    public override void OnTouchGround(object sender, EventArgs e)
    {
        
    }
}