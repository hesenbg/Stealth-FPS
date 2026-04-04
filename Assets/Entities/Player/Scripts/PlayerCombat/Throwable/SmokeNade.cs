using System;
using UnityEngine;
public class SmokeNade : BaseNade
{
    public  void ExecuteNadeLogic()
    {
        Destroy(gameObject);
    }

    public override void OnNadeActivated(object sender, EventArgs e)
    {

    }

    public override void OnNadeDeactivated(object sender, EventArgs e)
    {
        
    }
    public override void OnTouchGround(object sender, EventArgs e)
    {

    }

    void DisableVisionCones()
    {

    }

    void EnableVisionCones()
    {

    }
}