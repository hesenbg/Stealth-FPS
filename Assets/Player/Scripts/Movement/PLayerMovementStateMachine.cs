using UnityEngine;

public class PLayerMovementStateMachine : StateManager<PLayerMovementStateMachine.PlayerMovementState>
{
    public enum PlayerMovementState {Run, Walk, Crouch, Jump, Idle, Hook}


    [SerializeField] PlayerMovementData PlayerMovementData;

}
