using UnityEngine;

public class GunBlocked : GunState
{
    public GunBlocked(GunContext context) : base(context, GunStateMachine.GunState.Blocked)
    {
        GunContext context1 = context;
    }

    public override void OnStateEnter()
    {
        // Enter blocked state logic
    }

    public override void OnStateUpdate()
    {
        // Update blocked state logic
    }

    public override void OnStateExit()
    {
        // Exit blocked state logic
    }

    public override GunStateMachine.GunState GetNextState()
    {
        // Determine next state based on conditions
        // For now, return Idle when unblocked
        return GunStateMachine.GunState.Idle;
    }
}
