using System;
using UnityEngine;
using UnityEngine.Analytics;
/// casts sets of rays and returns weather player on sight or not.player cant be detected if there is no light illuminates it(light can smoehow illuminates the player cuz i cant communicate with gpu
/// sight is 135 euler degeers.(i am not sure)
public class GuardSight : MonoBehaviour
{
    [SerializeField] Vector3[] SightDirections;
    [SerializeField] float SightDistance;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;
    GuardingEnemy Enemy;
    
    public bool TargetOnSight = false;
    Vector3 LocalShightDirection;
    PlayerVisibility PlayerVisibility;
    private void Start()
    {
        

        PlayerVisibility = GameObject.Find("PlayerVisibility").GetComponent<PlayerVisibility>();
        Enemy = GetComponent<GuardingEnemy>();
    }
    public bool CanSeePlayer() => TargetOnSight;

    void LookAround()
    {
        TargetOnSight = false;

        foreach (var SightDirection in SightDirections)
        {
            LocalShightDirection = transform.TransformDirection(SightDirection);

            Ray Sight = new Ray(SightOrigin, LocalShightDirection);
            RaycastHit hitPlayer;
            RaycastHit ObstacleHit;

            if (Physics.Raycast(Sight, out hitPlayer,SightDistance,TargetMask) && PlayerVisibility.visible) // 
            {              
                float distanceToPlayer = hitPlayer.distance;

                TargetOnSight = false;

                if(!Physics.Raycast(Sight, out ObstacleHit, SightDistance, ObstacleMask))
                {
                    TargetOnSight = true;
                }
                Enemy.PlayerSpotPosition = hitPlayer.point;

            }
        }
    }
    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((1 << obj.layer) & mask) != 0;
    }


    public bool IsPlayerClose()
    {
        Collider[] AroundColliders;
        AroundColliders= Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in AroundColliders)
        {
            if (collider.gameObject.layer == 10)
            {
                return true;
            }
        }
        return false;
    }
    public void HasSeenDeadbody()
    {
        
    }

    private void FixedUpdate()
    {
        LookAround();
        SightOrigin = transform.position;
    }
    [SerializeField] Vector3 SightOrigin;
    Vector3 LocalGizmo;
    private void OnDrawGizmos()
    {
        if (SightDirections == null) return;
        Gizmos.color = Color.yellow;
        foreach (var dir in SightDirections)
        {
            LocalGizmo = transform.TransformDirection(dir);
            Gizmos.DrawRay(SightOrigin, LocalGizmo.normalized * SightDistance);
        }
    }
}
