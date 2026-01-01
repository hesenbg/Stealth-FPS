using UnityEngine;

public class GunReload : GunState
{
    private float reloadTimer;

    public GunReload(GunContext context) : base(context, GunStateMachine.GunState.Reload)
    {
        GunContext context1 = context;
    }

    public override void OnStateEnter()
    {
        reloadTimer = Context.GetReloadTime();
        // Enter reload state logic
    }

    public override void OnStateUpdate()
    {
        reloadTimer -= Time.deltaTime;
        // Update reload state logic
    }

    public override void OnStateExit()
    {
        // Exit reload state logic
    }

    public override GunStateMachine.GunState GetNextState()
    {
        // If reload is complete, return to Idle
        if (reloadTimer <= 0)
        {
            return GunStateMachine.GunState.Idle;
        }
        return GunStateMachine.GunState.Reload;
    }
}
