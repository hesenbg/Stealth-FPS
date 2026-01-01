using UnityEngine;

public class GunIdle : GunState
{
    public GunIdle(GunContext context) : base(context, GunStateMachine.GunState.Idle)
    {
        GunContext context1 = context;
    }

    public override void OnStateEnter()
    {
        // Enter idle state logic
    }

    public override void OnStateUpdate()
    {
        // Update idle state logic
    }

    public override void OnStateExit()
    {
        // Exit idle state logic
    }

    public override GunStateMachine.GunState GetNextState()
    {
        // Determine next state based on conditions
        // For now, return Idle to stay in this state
        return GunStateMachine.GunState.Idle;
    }
}
