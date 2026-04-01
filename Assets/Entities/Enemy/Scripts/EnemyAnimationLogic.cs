using System;
using UnityEngine;

public class EnemyAnimationLogic : MonoBehaviour
{
    Animator animator;
    public event EventHandler InvestigationEnd;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayInvestigate()
    {
        animator.SetTrigger("Investigate");
    }

    public void PlayLookAround()
    {
        animator.SetTrigger("LookAround");
    }

    public void ReturnBackLookAround()
    {
        animator.SetTrigger("ReturnBack");
    }

    public void PlayHoldAnimation(bool IsHolding)
    {
        animator.SetBool("Still", IsHolding);
    }

    public void PlayMovementAnimation(float VelocityMagnitute) // magnitute of x and z velocit axis
    {
        animator.SetFloat("Speed", VelocityMagnitute);
    }

    public void FireInvestigationEnd()
    {
        InvestigationEnd?.Invoke(this, EventArgs.Empty);
    }
}
