using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeNade : BaseNade
{
    [Header("Smoke properties")]
    [SerializeField] float SmokeDuration;
    [SerializeField] float SmokeObservability;
    [SerializeField] LayerMask TargetMask;

    private HashSet<ObservableObject> affectedObjects = new HashSet<ObservableObject>();
    private bool smokeActive = false;

    public void ExecuteNadeLogic()
    {
    }

    public override void Init()
    {
    }

    public override void OnNadeActivated(object sender, EventArgs e)
    {
    }

    public override void OnNadeUpdate()
    {
        if (!smokeActive) return;

        HashSet<ObservableObject> currentObjects = new HashSet<ObservableObject>();

        Collider[] hits = Physics.OverlapSphere(transform.position, EffectRadius, TargetMask);
        foreach (Collider col in hits)
        {
            if (col.TryGetComponent<ObservableObject>(out ObservableObject obs))
            {
                currentObjects.Add(obs);
                obs.AddModifier("Smoke", SmokeObservability);
            }
        }

        foreach (ObservableObject obs in affectedObjects)
        {
            if (!currentObjects.Contains(obs))
                obs.RemoveModifier("Smoke");
        }

        affectedObjects = currentObjects;
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

    void ExecuteNade()
    {
        smokeActive = true;
        ExecuteNadeEffects();
        StartCoroutine(SmokeEffectDuration());
        DisableNadePhysics();
        AllignNade();
    }

    void AllignNade()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    IEnumerator SmokeEffectDuration()
    {
        yield return new WaitForSeconds(SmokeDuration);
        smokeActive = false;
        ClearAllModifiers();
        Destroy(gameObject);
    }

    void ClearAllModifiers()
    {
        foreach (ObservableObject obs in affectedObjects)
            obs.RemoveModifier("Smoke");

        affectedObjects.Clear();
    }
}