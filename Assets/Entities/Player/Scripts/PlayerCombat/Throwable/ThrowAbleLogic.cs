using System;
using System.Collections;
using UnityEngine;

public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] FlashNade flashNade;

    [SerializeField] SmokeNade smokeNade;

    BaseNade CurrentNade;

    [SerializeField] Transform source;

    private void Update()
    {
        // current nade selection will be handled in here

    }

    private void Start()
    {
        CurrentNade = flashNade;
    }

    public void ThrowNade()
    {
        BaseNade ThrowenNade = Instantiate(CurrentNade, source.position,source.rotation);
    }
}
