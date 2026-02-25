using UnityEngine;
public class Recoil : MonoBehaviour
{
    private Vector3 CurrRotation;
    private Vector3 TargetRotation;
    [SerializeField] GameObject Mesh;
    [SerializeField] GameObject Camera;

    [SerializeField] float CameraEffectWeight;
    [SerializeField] float MeshEffectWeight;

    [SerializeField] float Snappines;
    [SerializeField] float returnSpeed;

    [SerializeField] float RecoilMultipiler;

    [SerializeField] Vector3 RecoilValue;

    private Vector3 StartValue;

    private void Start()
    {
        StartValue = Mesh.transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        // calcculate
        TargetRotation = Vector3.Lerp(TargetRotation, Vector3.zero , returnSpeed * Time.deltaTime);
        CurrRotation = Vector3.Slerp(CurrRotation, TargetRotation, Snappines * Time.deltaTime);

        // apply
        Camera.transform.localRotation = Quaternion.Euler(CurrRotation*CameraEffectWeight);
        Mesh.transform.localRotation = Quaternion.Euler(CurrRotation*MeshEffectWeight);
    }

    public void RecoilFire(Vector3 Recoil)
    {
        Recoil = RecoilValue;
        TargetRotation -= new Vector3(Recoil.y, Recoil.x,Recoil.z);
    }
}