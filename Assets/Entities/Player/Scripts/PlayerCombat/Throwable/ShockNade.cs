using System;
using UnityEngine;

public class ShockNade : BaseNade
{
    [SerializeField] float Damage;
    [SerializeField] float DeathClose;
    Vector3 dir;

    void ExecuteNadeLogic()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, EffectRadius);
        foreach (Collider col in colliders)
        {
            HealthManager health = col.GetComponentInChildren<HealthManager>();
            if (health == null) continue;

            dir = col.transform.position - transform.position;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, EffectRadius))
            {
                HealthManager hitHealth = hit.collider.GetComponentInChildren<HealthManager>();
                if (hitHealth == health)
                {
                    // call function
                } 
            }
        }
    }

    public override void Init()
    {

    }

    public override void OnNadeActivated(object sender, EventArgs e)
    {

    }

    public override void OnNadeDeactivated(object sender, EventArgs e)
    {

    }

    public override void OnTouchGround(object sender, EventArgs e)
    {
        ExecuteNadeEffects();
        ExecuteNadeLogic();
        Destroy(gameObject);
    }

    float CalculateDropOff(Vector3 pos , float damage)
    {
        float dist = Vector3.Distance(transform.position, pos);

        float dropOff = dist / EffectRadius;

        if(dist < DeathClose)
            return damage;
        return damage * (1-dropOff);
    }
}