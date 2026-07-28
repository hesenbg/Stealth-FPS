using UnityEngine;

public class DestructableHealthManager : HealthManager
{
    public override void ApplyDamageEffect(float EffectDuration)
    {
        
    }

    public override void ApplyFlashEffect(float EffectDuration, Vector3 Direction)
    {
        
    }

    public override void ApplyShockEffect(float EffectDuration)
    {
        
    }

    public override void OnDamage(float Damage, Vector3 pos)
    {
        
    }

    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}
