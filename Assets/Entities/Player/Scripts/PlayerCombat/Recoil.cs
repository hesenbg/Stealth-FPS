using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 CurrRotation;
    private Vector3 CurrPos;
    private Vector3 TargetRotation;
    private Vector3 TargetPos;

    bool _IsADS = false;

    private float CurrMeshEffectWeight;
    private float CurrCameraEffectWeight;

    [SerializeField] float rotMultipiler;

    [SerializeField] GameObject Mesh;
    [SerializeField] GameObject Camera;
    [SerializeField] Transform rigParent;

    [SerializeField] Vector3 rigParentOriginalRot;

    [SerializeField] float CameraEffectWeight;
    [SerializeField] float MeshEffectWeight;

    [SerializeField] float Snappines;
    [SerializeField] float returnSpeed;

    [SerializeField] float returnDelay;
    private float currentReturnTimer;

    [SerializeField] float ADSmultipiler;

    [SerializeField] Vector3 RecoilRotValue;
    [SerializeField] Vector3 RecoilPosValue;

    private bool isRecoiling;

    void Start()
    {
        CurrMeshEffectWeight = MeshEffectWeight;
        CurrCameraEffectWeight = CameraEffectWeight;
        rigParentOriginalRot = rigParent.localRotation.eulerAngles;
        TargetRotation = rigParentOriginalRot;
        CurrRotation = rigParentOriginalRot;
    }

    private void Update()
    {
        UpdatePos();
        UpdateRecoilRig();
        UpdateMeshRecoil();
        UpdateCameraRecoil();
    }

    void UpdatePos()
    {
        TargetPos = Vector3.Lerp(TargetPos, Vector3.zero, returnSpeed * Time.deltaTime);
        CurrPos = Vector3.Lerp(CurrPos, TargetPos, Snappines * Time.deltaTime);
    }

    void UpdateMeshRecoil()
    {
        Mesh.transform.localPosition = Vector3.Lerp(Mesh.transform.localPosition, CurrPos, returnSpeed * Time.deltaTime * MeshEffectWeight);
    }

    void UpdateCameraRecoil()
    {
        Vector3 recoilOffset = CurrRotation - rigParentOriginalRot;

        Quaternion targetCameraRot = Quaternion.Euler(recoilOffset.x * CurrCameraEffectWeight, recoilOffset.y * CurrCameraEffectWeight, recoilOffset.z * CurrCameraEffectWeight);

        Camera.transform.localRotation = Quaternion.Slerp(
            Camera.transform.localRotation,
            targetCameraRot,
            Snappines * Time.deltaTime
        );
    }

    void UpdateRecoilRig()
    {
        if (!isRecoiling) return;

        TargetRotation = Vector3.Slerp(TargetRotation, rigParentOriginalRot, returnSpeed * Time.deltaTime);
        CurrRotation = Vector3.Slerp(CurrRotation, TargetRotation, Snappines * Time.deltaTime);

        float distRot = Vector3.Distance(CurrRotation, rigParentOriginalRot);

        if (distRot < 0.1f)
        {
            CurrRotation = rigParentOriginalRot;
            TargetRotation = rigParentOriginalRot;
            GunRigController.Instance.ApplyRotationWeight(CurrRotation * rotMultipiler, _IsADS ? 1f : 0f);
            isRecoiling = false;
        }
        else
        {
            GunRigController.Instance.ApplyRotationWeight(CurrRotation * rotMultipiler, 1f);
        }
    }

    public void RecoilFire(bool IsADS)
    {
        _IsADS = IsADS;
        isRecoiling = true;
        currentReturnTimer = returnDelay;

        TargetRotation -= new Vector3(RecoilRotValue.y,
            Random.Range(-RecoilRotValue.x, RecoilRotValue.x),
            RecoilRotValue.z);

        TargetPos -= RecoilPosValue;

        if (IsADS)
        {
            CurrMeshEffectWeight = MeshEffectWeight;
            CurrCameraEffectWeight = CameraEffectWeight * ADSmultipiler;
        }
        else
        {
            CurrCameraEffectWeight = CameraEffectWeight;
            CurrMeshEffectWeight = MeshEffectWeight;
        }
    }

    public void RecoilReset()
    {
        TargetRotation = rigParentOriginalRot;
        CurrRotation = rigParentOriginalRot;
        TargetPos = Vector3.zero;
        CurrPos = Vector3.zero;
        currentReturnTimer = 0;

        GunRigController.Instance.ApplyRotationWeight(CurrRotation * rotMultipiler, 0f);
        isRecoiling = false;

        // Reset camera to neutral
        Camera.transform.localRotation = Quaternion.identity;
    }
}