using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 CurrRotation;
    private Vector3 CurrPos;
    private Vector3 TargetRotation;
    private Vector3 TargetPos;

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

    void Start()
    {
        CurrMeshEffectWeight = MeshEffectWeight;
        CurrCameraEffectWeight = CameraEffectWeight;
        rigParentOriginalRot = rigParent.localRotation.eulerAngles;
        TargetRotation = rigParentOriginalRot; // add this
        CurrRotation = rigParentOriginalRot;   // add this
    }


    private void Update()
    {
        TargetPos = Vector3.Lerp(TargetPos, Vector3.zero, returnSpeed * Time.deltaTime);
        CurrPos = Vector3.Lerp(CurrPos, TargetPos, Snappines * Time.deltaTime);



        CurrRotation = Vector3.Slerp(CurrRotation, TargetRotation, Snappines * Time.deltaTime);
        TargetRotation = Vector3.Slerp(TargetRotation, rigParentOriginalRot, returnSpeed * Time.deltaTime);

        //Camera.transform.localRotation = Quaternion.Euler(CurrRotation * CurrCameraEffectWeight);
        //Mesh.transform.localRotation = Quaternion.Euler(CurrRotation * CurrMeshEffectWeight);

        //Mesh.transform.localPosition = CurrPos;

        float recoilWeight = Vector3.Distance(CurrRotation, rigParentOriginalRot) < 0.1f ? 0f : 1f;
        GunRigController.Instance.ApplyRotationWeight(CurrRotation * rotMultipiler, recoilWeight);
    }

    public void RecoilFire(bool IsADS)
    {
        currentReturnTimer = returnDelay;

        TargetRotation -= new Vector3(RecoilRotValue.y,
            Random.Range(-RecoilRotValue.x, RecoilRotValue.x),
            RecoilRotValue.z);

        TargetPos -= RecoilPosValue;

        if (IsADS)
        {
            CurrMeshEffectWeight = MeshEffectWeight * 0f;
            CurrCameraEffectWeight = CameraEffectWeight * ADSmultipiler;
        }
        else
        {
            CurrCameraEffectWeight = CameraEffectWeight;
            CurrMeshEffectWeight = MeshEffectWeight;
        }
    }
}