using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunRigController : MonoBehaviour
{
    public static GunRigController Instance { get; private set; }

    [SerializeField] Rig GunRig;
    [SerializeField] public Transform RigParent;

    private Vector3 OriginalPos;

    private Quaternion OriginalRot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OriginalPos = RigParent.localPosition;
        OriginalRot = RigParent.localRotation;
    }

    public void ApplyPosWeigth(Vector3 pos, float weight)
    {
        GunRig.weight = weight;
        RigParent.localPosition = pos;
    }

    public void ApplyPos(Vector3 pos)
    {
        RigParent.localPosition = pos;
    }

    public void ApplyRot(Vector3 rot)
    {
        RigParent.localRotation = Quaternion.Euler(rot);
    }

    public void ApplyRotationWeight(Vector3 rot, float weight)
    {
        RigParent.localRotation = Quaternion.Euler(rot);
        GunRig.weight = weight;
    }

    public void ResetRigPos()
    {
        GunRig.weight = 0f;
        RigParent.localPosition = OriginalPos;
    }

    public void ResetRigRot()
    {
        GunRig.weight = 0f;
        RigParent.localRotation = OriginalRot;
    }

    public void ResetRig()
    {
        ResetRigPos();
        ResetRigRot();
    }

    public void SetTargetLocalPos(Vector3 localPos)
    {
        GunRig.weight = 1f;
    }
}