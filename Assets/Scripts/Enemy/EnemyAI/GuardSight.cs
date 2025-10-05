using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GuardSight : MonoBehaviour
{

    [SerializeField]
    Vector3[] SightDirections = new Vector3[]
    {
        new Vector3(0, 0, 1).normalized,
        new Vector3(1, 0, 1).normalized,
        new Vector3(-1, 0, 1).normalized,
        new Vector3(-1, 0, 0.5f).normalized,
        new Vector3(1, 0, 0.5f).normalized,
        new Vector3(0.5f, 0, 1).normalized,
        new Vector3(-0.5f, 0, 1).normalized,
        new Vector3(-0.25f, 0, 1).normalized,
        new Vector3(0.25f, 0, 1).normalized,
        new Vector3(1.5f, 0, 1).normalized,
        new Vector3(-1.5f, 0, 1).normalized,
        new Vector3(-0.75f, 0, 1).normalized,
        new Vector3(0.75f, 0, 1).normalized,
        new Vector3(0.125f, 0, 1).normalized,
        new Vector3(-0.125f, 0, 1).normalized,
        new Vector3(-1.25f, 0, 1).normalized,
        new Vector3(1.25f, 0, 1).normalized,
        new Vector3(0.625f, 0, 1).normalized,
        new Vector3(-0.625f, 0, 1).normalized,
        new Vector3(-0.25f, 0, 0.7f).normalized,
        new Vector3(0.25f, 0, 0.7f).normalized,
        new Vector3(0.125f, 0, 0.7f).normalized,
        new Vector3(-0.125f, 0, 0.7f).normalized,
        new Vector3(-0.125f, 0, 1.8f).normalized,
        new Vector3(0.07f, 0, 1).normalized,
        new Vector3(1, 0, 2.4f).normalized,
        new Vector3(-1, 0, 2.4f).normalized,
        new Vector3(-1, 0, 1.16f).normalized,
        new Vector3(1, 0, 1.16f).normalized
    };

    Vector3[] WorldSightDirections;               // runtime world-space
    [SerializeField] float[] YOffsets;           // scanning offsets

    [SerializeField] float SightDistance;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;
    [SerializeField] GameObject Origin;

    BaseAI Enemy;
    PlayerVisibility PlayerVisibility;

    public bool TargetOnSight = false;

    [SerializeField] float SightLowerIncrement = 1f;
    [SerializeField] Vector3 SightOrigin;
    [SerializeField] float MaxSightLowerValue;
    [SerializeField] float LowerCheckSightDistance;

    private void Start()
    {
        PlayerVisibility = GameObject.Find("PlayerVisibility").GetComponent<PlayerVisibility>();
        Enemy = GetComponent<BaseAI>();
        WorldSightDirections = new Vector3[SightDirections.Length];
        YOffsets = new float[SightDirections.Length];
    }

    public bool CanSeePlayer() => TargetOnSight;

    void UpdateWorldSightDirections()
    {
        if (SightDirections == null) return;

        for (int i = 0; i < SightDirections.Length; i++)
        {
            // transform the base local direction
            Vector3 dir = transform.TransformDirection(SightDirections[i]);

            // apply Y offset (scanning downward)
            dir.y += YOffsets[i]*Time.deltaTime;

            WorldSightDirections[i] = dir.normalized;
        }
    }

    void LookAround()
    {
        TargetOnSight = false;

        for (int i = 0; i < WorldSightDirections.Length; i++)
        {
            Vector3 dir = WorldSightDirections[i];
            Ray ray = new Ray(SightOrigin, dir);

            // check dead body
            // check ground
            if (Physics.Raycast(ray, out RaycastHit DeadBodyHit, SightDistance))
            {
                if (DeadBodyHit.collider.gameObject.layer == 6) // ground layer
                {

                }
            }

            // check ground
            if (Physics.Raycast(ray, out RaycastHit groundHit, LowerCheckSightDistance))
            {                                  // ground layer groundHit.collider.gameObject.layer == 6 ||
                if ( Mathf.Abs(YOffsets[i])>MaxSightLowerValue) 
                {
                    YOffsets[i] = 0;
                    continue; // skip player
                }
            }

            YOffsets[i] -= Time.deltaTime * SightLowerIncrement;

            // check player visibility
            if (Physics.Raycast(ray, out RaycastHit hitPlayer, SightDistance, TargetMask) && PlayerVisibility.visible)
            {
                if (!Physics.Raycast(ray, SightDistance, ObstacleMask))
                {
                    TargetOnSight = true;
                    Enemy.PlayerSpotPosition = hitPlayer.point;
                }
            }
        }
    }

    [SerializeField] float SightOriginIncrement;
    private void FixedUpdate()
    {
        SightOrigin =  new Vector3(Origin.transform.position.x,
            Origin.transform.position.y+SightOriginIncrement,
            Origin.transform.position.z
            );
        SightOrigin = Origin.transform.position;
        UpdateWorldSightDirections();
        LookAround();

    }

    private void OnDrawGizmos()
    {
        if (WorldSightDirections == null || WorldSightDirections.Length == 0) return;

        Gizmos.color = Color.black;
        for (int i = 0; i < WorldSightDirections.Length; i++)
        {
            Gizmos.DrawRay(Origin.transform.position, WorldSightDirections[i] * SightDistance);
            Gizmos.color = Color.white;
            Gizmos.DrawRay(Origin.transform.position, WorldSightDirections[i] * LowerCheckSightDistance);

        }
    }




}
