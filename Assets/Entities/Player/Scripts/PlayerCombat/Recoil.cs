using UnityEngine;
public class Recoil : MonoBehaviour
{
    private Vector3 CurrRotation;
    private Vector3 CurrPos;
    private Vector3 TargetRotation;
    private Vector3 TargetPos;
    [SerializeField] GameObject Mesh;
    [SerializeField] GameObject Camera;

    [SerializeField] float CameraEffectWeight;
    [SerializeField] float MeshEffectWeight;

    [SerializeField] float Snappines;
    [SerializeField] float returnSpeed;

    [SerializeField] float RecoilMultipiler;

    [SerializeField] Vector3 RecoilRotValue;

    [SerializeField] Vector3 RecoilPosValue;


    private void Start()
    {
    }

    private void Update()
    {
        // calcculate rot 
        TargetRotation = Vector3.Slerp(TargetRotation, Vector3.zero , returnSpeed * Time.deltaTime);
        CurrRotation = Vector3.Slerp(CurrRotation, TargetRotation, Snappines * Time.deltaTime);

        // calculate pos
        TargetPos = Vector3.Lerp(TargetPos,Vector3.zero, returnSpeed * Time.deltaTime);
        CurrPos = Vector3.Lerp(CurrPos, TargetPos, Snappines * Time.deltaTime);

        // apply
        Camera.transform.localRotation = Quaternion.Euler(CurrRotation*CameraEffectWeight);
        Mesh.transform.localRotation = Quaternion.Euler(CurrRotation*MeshEffectWeight);

        Mesh.transform.localPosition = CurrPos;
    }

    public void RecoilFire()
    {
        TargetRotation -= new Vector3(RecoilRotValue.y,
            Random.Range(-RecoilRotValue.x, RecoilRotValue.x),
            RecoilRotValue.z);
        TargetPos -= RecoilPosValue;
    }
}