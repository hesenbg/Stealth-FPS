using UnityEngine;

public class PlayerState : MonoBehaviour
{
    PlayerInput input;

    public enum GunState {Idle, WallBlocked, Reload, Shoot, ADS, Unreachable}
    public enum MovementState { Idle, Walk, Run, Airbone}


    public GunState CurrentGunState;
    public MovementState CurrentMovementState;


    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    void UpdateMovementState()
    {

    }

    void UpdateGunState()
    {

    }

    private void Update()
    {
        
    }
}
