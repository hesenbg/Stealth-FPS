using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Player Movement Data")]
public class MovementData : ScriptableObject
{
    [Header("Max Movement Speeds")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float hookSpeed;

    [Header("Movement Accelerations")]
    [SerializeField] float walkAcceleration;
    [SerializeField] float runAcceleration;
    [SerializeField] float crouchAcceleration;
    [SerializeField] float jumpAcceleration;
    [SerializeField] float hookAcceleration;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpMass;

    [Header("Crouch Hitbox")]
    [SerializeField] float crouchHitboxHeight;
    [SerializeField] float crouchLerpSpeed;
    [SerializeField] Vector3 crouchGroundCheck;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float CrouchSpeed => crouchSpeed;
    public float HookSpeed => hookSpeed;
    public float HookAcceleration => hookAcceleration;


    public float WalkAcceleration => walkAcceleration;
    public float RunAcceleration => runAcceleration;
    public float CrouchAcceleration => crouchAcceleration;
    public float JumpAcceleration => jumpAcceleration;

    public float JumpForce => jumpForce;
    public float JumpMass => jumpMass;

    public float CrouchHitboxHeight => crouchHitboxHeight;
    public float CrouchLerpSpeed => crouchLerpSpeed;
    public Vector3 CrouchGroundCheck => crouchGroundCheck;
}
