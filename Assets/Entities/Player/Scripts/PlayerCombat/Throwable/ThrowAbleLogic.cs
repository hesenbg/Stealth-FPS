using System;
using System.Collections;
using UnityEngine;

public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] ShockNade Shock;

    [SerializeField] SmokeNade smokeNade;

    BaseNade CurrentNade;

    private void Update()
    {
        // current nade selection will be handled in here
    }

    private void Start()
    {
        CurrentNade = Shock;
    }

    public void ThrowNadeLong()
    {
        BaseNade ThrownNade = Instantiate(CurrentNade, transform.position,transform.rotation);
    }
}
