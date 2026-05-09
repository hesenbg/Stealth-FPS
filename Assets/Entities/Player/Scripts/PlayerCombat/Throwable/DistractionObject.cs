using System;
using UnityEngine;
public class DistractionObject : BaseNade
{
    [SerializeField] HearableObject hearable;

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
        EnemyManager.instance.AlertClosestOnSuspiciousEvent(transform.position, hearable);
        Destroy(gameObject);
    }
}