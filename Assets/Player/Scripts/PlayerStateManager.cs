using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerInput input;

    public enum GunState {Idle, WallBlocked, Reload, Shoot, ADS, Unreachable}
    public enum MovementState { Idle, Walk, Run, Airbone}

    StateManager<MovementState> MovementStateManager;

    StateManager<GunState> GunStateManager;

    public GunState CurrentGunState;
    public MovementState CurrentMovementState;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }
}