using System;
using System.Collections;
using UnityEngine;
using static AwarenessFSM;

public class EnemyHealthManager : HealthManager
{
    [SerializeField] Transform Head;
    [SerializeField] GameObject OriginalHips;
    [SerializeField] GameObject CoreObjectToBeDestroyed;
    [SerializeField] GameObject RagdollObject;
    [SerializeField] EnemyEvents EnemyEvents;

    public EnemyAIData data;
    [SerializeField] EnemyUI UI;

    EnemyRagdoll ragdoll;
    bool hasKnifed = false;

    protected override void Start()
    {
        if (RagdollObject != null)
        {
            ragdoll = RagdollObject.GetComponent<EnemyRagdoll>();
        }
        base.Start();
    }


    public override void OnDamage(float damage, Vector3 pos)
    {
        if(damage>=MaxHealth)
            GetHeadShotDamage();
        EnemyVisualAudios.instance.PlayBodyHit(transform.position);
        EnemyVisualAudios.instance.PlayBloodVFX(Head.position);
    }

    public void GetHeadShotDamage()
    {
        ApplyLethalDamage();
        EnemyVisualAudios.instance.PlayHeadHit(transform.position);
    }

    public override void ApplyKnifeDamage()
    {
        hasKnifed = true;

        ragdoll.hasKnifed = hasKnifed;

        ApplyLethalDamage();
    }

    public override void OnDeath()
    {
        EnemyRagdoll spawnedRagdoll = SpawnRagdoll();
        if (spawnedRagdoll != null)
            spawnedRagdoll.hasKnifed = hasKnifed;
        Destroy(CoreObjectToBeDestroyed);
    }

    private EnemyRagdoll SpawnRagdoll()
    {
        if (RagdollObject == null || OriginalHips == null) return null;
        GameObject spawnedRagdoll = Instantiate(RagdollObject, transform.position, transform.rotation);
        EnemyRagdoll ragdollScript = spawnedRagdoll.GetComponent<EnemyRagdoll>();
        if (ragdollScript != null)
            ragdollScript.MatchRagdollToAnimation(OriginalHips);
        return ragdollScript;
    }

    public override void ApplyFlashEffect(float EffectDuration, Vector3 Direction) 
    {
        UI.FlashEffectUI();
        StartCoroutine(FlashEffectRoutine(EffectDuration*Vector3.Dot(Direction, transform.forward)));
    }

    private IEnumerator FlashEffectRoutine(float duration)
    {
        EnemyAwarnesParams original = data.CurrentAwarenessState;
        data.CurrentAwarenessState = EnemyAwarnesParams.Zero();
        yield return new WaitForSeconds(duration);
        data.CurrentAwarenessState = original;
        UI.DeActiveFlashEffectUI();
        EnemyEvents.FireClueFound(new EventData(transform.position,transform.forward));
        InvokeOnEffectEnds();
    }

    public override void ApplyDamageEffect(float EffectDuration)
    {
        
    }

    public override void ApplyShockEffect(float EffectDuration)
    {
        
    }
}