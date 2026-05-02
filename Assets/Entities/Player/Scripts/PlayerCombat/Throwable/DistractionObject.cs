using System;
public class DistractionObject : BaseNade
{
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
        EnemyManager.instance.AlertClosestSuspicious(transform.position);
        Destroy(gameObject);
    }
}