using System;
using UnityEngine;
public abstract class HealthManager : MonoBehaviour
{
    public float MaxHealth ;

    public float CurrentHealth {  get; private set; }

    protected float BaseDamageMultipiler = 1f;

    protected Action OnHealthZero;

    protected Action<float,Vector3> OnDamageDone;

    public event EventHandler OnEffectEnds;

    public virtual void ApplyDamage(float damage, float Multipiler,Vector3 pos)
    {
        var totalDamage = damage * Multipiler * BaseDamageMultipiler;

        CurrentHealth -= totalDamage;
        OnDamageDone?.Invoke(totalDamage,pos);

        CheckDie();
    }

    public void InvokeOnEffectEnds()
    {
        OnEffectEnds?.Invoke(this, EventArgs.Empty);
    }

    public abstract void ApplyFlashEffect(float EffectDuration, Vector3 Direction);

    public abstract void ApplyDamageEffect(float EffectDuration);

    public abstract void ApplyShockEffect(float EffectDuration);

    public void ApplyLethalDamage()
    {
        CurrentHealth = 0;
        CheckDie();
    }

    bool CheckDie()
    {
        if (CurrentHealth <= 0f)
        {
            OnHealthZero?.Invoke();
            OnHealthZero = null;
            return true;
        }
        return false;
    }

    public void IncreaseHealth(float Health)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + Health, 0f, MaxHealth);
    }

    virtual public void ApplyKnifeDamage()
    {

    }

    abstract public void OnDeath();

    abstract public void OnDamage(float Damage,Vector3 pos);

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
        OnHealthZero += OnDeath;
        OnDamageDone += OnDamage;
    }
}