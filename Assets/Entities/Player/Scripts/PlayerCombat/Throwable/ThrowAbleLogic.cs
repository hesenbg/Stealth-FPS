using System;
using System.Collections;
using UnityEngine;

public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] FlashNade flashNade;

    [SerializeField] SmokeNade smokeNade;

    BaseNade CurrentNade;

    private void Update()
    {
        // current nade selection will be handled in here
    }

    private void Start()
    {
        CurrentNade = flashNade;
    }

    public void ThrowNadeShort()
    {
        BaseNade ThrownNade = Instantiate(CurrentNade, transform.position, transform.rotation);
    }


    public void ThrowNadeLong()
    {
        BaseNade ThrownNade = Instantiate(CurrentNade, transform.position,transform.rotation);
    }
}
