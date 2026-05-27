using System;
using System.Collections;
using UnityEngine;

public class SmokeNade : BaseNade
{
    [Header("Smoke propeties")]
    [SerializeField] float SmokeDuration;

    public void ExecuteNadeLogic()
    {

    }

    public override void Init()
    {

    }

    public override void OnNadeActivated(object sender, EventArgs e)
    {

    }

    void ExecuteNade()
    {
        ExecuteNadeEffects();
        StartCoroutine(SmokeEffectDuration());
        DisableNadePhysics();
        AllignNade();
    }

    public override void OnNadeUpdate()
    {

    }

    public override void OnNadeDeactivated(object sender, EventArgs e)
    {
    }

    public override void OnTouchGround(object sender, EventArgs e)
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(FuseTimer);
        ExecuteNade();
    }

    void AllignNade()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    IEnumerator SmokeEffectDuration()
    {
        yield return new WaitForSeconds(SmokeDuration);
        Destroy(gameObject);
    }

}