using UnityEngine;
using UnityEngine.InputSystem;
public abstract class GunState : BaseState<GunStateMachine.GunState>
{
    protected GunContext Context;

    public GunState(GunContext context,GunStateMachine.GunState StateKey ) :base(StateKey)
    {
        Context = context;
    }
}
