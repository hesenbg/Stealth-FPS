using UnityEngine;
using System;
using TMPro;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField] Grenade Smoke;
    [SerializeField] Grenade Explosive;
    [SerializeField] Transform Player;
    [HideInInspector] public TextMeshProUGUI GrenadeCountUI;
    [SerializeField] Camera MainCamera;
    [SerializeField] float Force;
    [SerializeReference] float ExplosionForce;
    [SerializeField] float FuseDelay;
    public int SmokeCount;
    public int ExplosiveCount;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && SmokeCount>0)
        {
            SmokeCount--;
            Smoke.Force = Force;
            Smoke.ThrowDirection = MainCamera.transform.forward;    
            Instantiate(Smoke,transform.position,transform.rotation);   
        }

        if (Input.GetKeyDown(KeyCode.G) && ExplosiveCount >0)
        {
            ExplosiveCount--;
            Explosive.Force = ExplosionForce;
            Explosive.FuseDelay  = FuseDelay;
            Explosive.ThrowDirection = MainCamera.transform.forward;
            Instantiate(Explosive,transform.position,transform.rotation);
        }

        //GrenadeCountUI.text = $"{GrenadeCount}";
    }
}
