using UnityEngine;

public class GunShoot : GunState
{
    public GunShoot(GunContext context) : base(context, GunStateMachine.GunState.Shoot)
    {
        GunContext context1 = context;
    }

    public override void OnStateEnter()
    {
        // Enter shoot state logic
    }

    public override void OnStateUpdate()
    {
        // Update shoot state logic
    }

    public override void OnStateExit()
    {
        // Exit shoot state logic
    }

    public override GunStateMachine.GunState GetNextState()
    {
        // Determine next state based on conditions
        // For now, return Idle after shooting
        return GunStateMachine.GunState.Idle;
    }
}
