using System.Collections;
using UnityEngine;
public class PlayerHealthManager : HealthManager
{


    protected override void Start()
    {
        base.Start();
        PlayerComponents.Instance.PlayerUI.SetPlayerDamageUI(1f - (float)CurrentHealth / MaxHealth);
    }

    private void Update()
    {
        PlayerComponents.Instance.PlayerUI.SetPlayerDamageUI(1f - (float)CurrentHealth / MaxHealth);
    }

    public override void OnDeath()
    {
        
    }

    public override void OnDamage(float damage, Vector3 pos)
    {
        PlayerComponents.Instance.recoil.ShakeCamera(damage);

        PlayerComponents.Instance.PlayerUI.SetPlayerDamageUI(1f - (float)CurrentHealth / MaxHealth);    
    }

    public override void ApplyFlashEffect(float EffectDuration, Vector3 Direction)
    {
        PlayerComponents.Instance.PlayerUI.FlashEffectUI(EffectDuration* Vector3.Dot(Direction, transform.forward));
    }

    public override void ApplyDamageEffect(float EffectDuration)
    {
        
    }



    public override void ApplyShockEffect(float EffectDuration)
    {
        
    }
}