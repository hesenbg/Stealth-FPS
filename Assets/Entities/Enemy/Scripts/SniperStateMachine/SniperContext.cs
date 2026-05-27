using Unity.Mathematics;
using UnityEngine;

public class SniperContext
{
    private GameObject controller;
    private EnemyEvents events;
    private VisionCone sight;
    private EnemyHealthManager healthManager;
    private GameObject parent;
    private EnemyAIData data;
    private EnemyCombatLogic enemyCombatLogic;
    private SniperStateMachine sfm;

    public SniperContext(GameObject controller, EnemyEvents events, VisionCone sight,
        EnemyHealthManager healthManager, GameObject parent, EnemyAIData data, EnemyCombatLogic enemyCombatLogic, SniperStateMachine sFM)
    {
        this.controller = controller;
        this.events = events;
        this.sight = sight;
        this.healthManager = healthManager;
        this.parent = parent;
        this.data = data;
        this.enemyCombatLogic = enemyCombatLogic;
        sfm = sFM;
    }
    public SniperStateMachine SFM => sfm;
    public GameObject GetController => controller;
    public EnemyEvents GetEvents => events;
    public VisionCone GetSight => sight;
    public EnemyHealthManager GetHealthManager => healthManager;
    public GameObject GetParent => parent;
    public EnemyAIData GetData => data;
    public EnemyCombatLogic GetEnemyCombatLogic => enemyCombatLogic;

    public bool UpdateRotation(Vector3 direction, float speed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rot = Quaternion.Slerp(sight.transform.rotation, targetRotation, speed * Time.deltaTime);
        sight.transform.rotation = rot;
        controller.transform.rotation = rot;
        return Quaternion.Angle(rot, targetRotation) < 3f;
    }
}