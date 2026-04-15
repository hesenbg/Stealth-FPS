using System;
using System.Collections;
using UnityEngine;
public class SmokeNade : BaseNade
{
    [Header("Smoke propeties")]
    [SerializeField] float SmokeDuration;

    public  void ExecuteNadeLogic()
    {
        
    }

    public override void Init()
    {
    }

    public override void OnNadeActivated(object sender, EventArgs e)
    {
        DisableNadePhysics();
        DisableVisionCones();
        SmokeEffectDuration();
    }

    public override void OnNadeDeactivated(object sender, EventArgs e)
    {
        
    }
    public override void OnTouchGround(object sender, EventArgs e)
    {

    }

    IEnumerator SmokeEffectDuration()
    {
        yield return new WaitForSeconds(SmokeDuration);
        EnableVisionCones();
        Destroy(gameObject);
    }

    void DisableVisionCones()
    {

    }

    void EnableVisionCones()
    {

    }
}