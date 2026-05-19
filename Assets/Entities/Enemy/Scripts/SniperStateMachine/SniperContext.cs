using UnityEngine;

public class SniperContext
{
    private SniperProceduralController controller;
    private EnemyEvents events;
    private VisionCone sight;
    private HealthManager healthManager;
    private GameObject parent;
    private EnemyAIData data;
    private EnemyCombatLogic enemyCombatLogic;
    private SniperStateMachine sfm;

    public SniperContext(SniperProceduralController controller, EnemyEvents events, VisionCone sight,
        HealthManager healthManager, GameObject parent, EnemyAIData data, EnemyCombatLogic enemyCombatLogic, SniperStateMachine sFM)
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
    public SniperProceduralController GetController => controller;
    public EnemyEvents GetEvents => events;
    public VisionCone GetSight => sight;
    public HealthManager GetHealthManager => healthManager;
    public GameObject GetParent => parent;
    public EnemyAIData GetData => data;
    public EnemyCombatLogic GetEnemyCombatLogic => enemyCombatLogic;
}