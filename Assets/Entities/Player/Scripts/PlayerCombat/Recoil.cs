using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 CurrRotation;
    private Vector3 CurrPos;
    private Vector3 TargetRotation;
    private Vector3 TargetPos;

    private float CurrMeshEffectWeight;
    private float CurrCameraEffectWeight;

    [SerializeField] GameObject Mesh;
    [SerializeField] GameObject Camera;

    [SerializeField] float CameraEffectWeight;
    [SerializeField] float MeshEffectWeight;

    [SerializeField] float Snappines;
    [SerializeField] float returnSpeed;

    [SerializeField] float returnDelay;
    private float currentReturnTimer;

    [SerializeField] float ADSmultipiler;

    [SerializeField] Vector3 RecoilRotValue;

    [SerializeField] Vector3 RecoilPosValue;

    private void Start()
    {
        CurrMeshEffectWeight = MeshEffectWeight;
        CurrCameraEffectWeight = CameraEffectWeight;
    }


    private void Update()
    {
        if (currentReturnTimer > 0)
        {
            currentReturnTimer -= Time.deltaTime;
        }
        else
        {
            TargetRotation = Vector3.Slerp(TargetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        }
        TargetPos = Vector3.Lerp(TargetPos, Vector3.zero, returnSpeed * Time.deltaTime);

        CurrRotation = Vector3.Slerp(CurrRotation, TargetRotation, Snappines * Time.deltaTime);
        CurrPos = Vector3.Lerp(CurrPos, TargetPos, Snappines * Time.deltaTime);

        Camera.transform.localRotation = Quaternion.Euler(CurrRotation * CurrCameraEffectWeight);
        Mesh.transform.localRotation = Quaternion.Euler(CurrRotation * CurrMeshEffectWeight);

        Mesh.transform.localPosition = CurrPos;
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