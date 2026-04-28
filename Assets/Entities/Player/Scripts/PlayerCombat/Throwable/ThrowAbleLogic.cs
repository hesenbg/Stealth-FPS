using System;
using System.Collections;
using UnityEngine;

public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] ShockNade Shock;

    [SerializeField] SmokeNade smokeNade;

    [SerializeField] DistractionObject DistractionNade;

    BaseNade CurrentNade;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) CurrentNade = Shock;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) CurrentNade = smokeNade;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) CurrentNade = DistractionNade;
    }   

    private void Start()
    {
        CurrentNade = DistractionNade;
    }

    public void ThrowNadeLong()
    {
        BaseNade ThrownNade = Instantiate(CurrentNade, transform.position,transform.rotation);
    }
}
