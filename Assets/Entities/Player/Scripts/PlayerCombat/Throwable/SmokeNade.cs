using System;
using System.Collections;
using UnityEngine;
public class SmokeNade : BaseNade
{
    [Header("Smoke propeties")]
    [SerializeField] float SmokeDuration;

    VisionCone[] enemies;

    public  void ExecuteNadeLogic()
    {
        
    }

    public override void Init()
    {
        enemies = EnemyManager.instance.GetEnemiesRange<VisionCone>(transform.position, EffectRadius);
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
        foreach(var enemy in enemies)
        {
            enemy.enabled = false;  
        }
    }

    void EnableVisionCones()
    {
        foreach (var enemy in enemies)
        {
            enemy.enabled = true;
        }
    }
}