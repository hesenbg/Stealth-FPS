using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunRigController : MonoBehaviour
{
    public static GunRigController Instance { get; private set; }
    [SerializeField] Rig GunRig;
    [SerializeField] public Transform RigParent;

    [Header("SOD Position")]
    [SerializeField] float posF = 2f, posZ = 0.8f, posR = 0f;
    [Header("SOD Rotation")]
    [SerializeField] float rotF = 2f, rotZ = 0.8f, rotR = 0f;

    private MathFunc.SODState posState;
    private MathFunc.SODState rotState;

    private Vector3 targetPos;
    private Vector3 targetRot;
    private float targetWeight;

    private void Awake()
    {
        Instance = this;
    }

    private void OnValidate()
    {
        posState = MathFunc.SODCreate(posF, posZ, posR, RigParent != null ? RigParent.localPosition : Vector3.zero);
        rotState = MathFunc.SODCreate(rotF, rotZ, rotR, RigParent != null ? RigParent.localEulerAngles : Vector3.zero);
    }

    private void Start()
    {
        posState = MathFunc.SODCreate(posF, posZ, posR, RigParent.localPosition);
        rotState = MathFunc.SODCreate(rotF, rotZ, rotR, RigParent.localEulerAngles);
        targetPos = RigParent.localPosition;
        targetRot = RigParent.localEulerAngles;
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        RigParent.localPosition = MathFunc.SODUpdate(ref posState, dt, targetPos);
        RigParent.localRotation = Quaternion.Euler(MathFunc.SODUpdate(ref rotState, dt, targetRot));
        GunRig.weight = targetWeight;
    }

    public void ApplyPosWeigth(Vector3 pos, float weight)
    {
        targetPos = pos;
        targetWeight = weight;
    }

    public void ApplyRot(Vector3 rot)
    {
        targetRot = rot;
    }

    public void ApplyRotationWeight(Vector3 rot, float weight)
    {
        targetRot = rot;
        targetWeight = weight;
    }
}