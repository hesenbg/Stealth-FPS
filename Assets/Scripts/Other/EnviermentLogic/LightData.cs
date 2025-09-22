using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class LightData : MonoBehaviour
{
    [SerializeField] Light LampLight;
    public bool IsPlayerIlluminated= false;
    Collider[] IlluminatedObjects;
    RaycastHit[] RaycastHitObjects;
    [SerializeField] LayerMask PlayerLayerMask;
    [SerializeField] LayerMask ObjectMask;
    GameObject PlayerGameObject;
    PlayerVisibility PlayerVisibility;
    

    [SerializeField] List<Vector3> LightRays;
    private void Start()
    {
        PlayerGameObject = GameObject.FindWithTag("Player");
        PlayerVisibility = GameObject.Find("PlayerVisibility").GetComponent<PlayerVisibility>();

        LampLight = GetComponent<Light>();
    }
    void CheckPlayerOnLight()
    {
        IsPlayerIlluminated = false; // reset before checking

        foreach (var dir in LightRays)
        {
            Ray ray = new Ray(transform.position,dir.normalized);

            if (Physics.Raycast(ray, out RaycastHit hit, LampLight.range, PlayerLayerMask))
            {
                if (!Physics.Raycast(ray, LampLight.range, ObjectMask))
                {
                    IsPlayerIlluminated = true;
                }
            }
        }
    }

    [SerializeField] Vector3 Origin;
    [SerializeField] Vector3 Direction;
    [SerializeField] float Radius;

    private void Update()
    {
        CheckPlayerOnLight();
        PlayerVisibility.visible = IsPlayerIlluminated;
    }

    private void OnDrawGizmosSelected()
    {
        if (LampLight == null) return;

        Gizmos.color = Color.yellow;

        foreach(var dir in LightRays)
        {

            Gizmos.DrawLine(transform.position,transform.position+dir*LampLight.range);
        }
    }

}
