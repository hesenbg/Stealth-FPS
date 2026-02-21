using UnityEngine;
public class Recoil : MonoBehaviour
{
    private Vector3 CurrRotation;
    private Vector3 TargetRotation;
    [SerializeField] GameObject AffectedObject;

    [SerializeField] float Snappines;
    [SerializeField] float returnSpeed;

    [SerializeField] float RecoilMultipiler;

    private void Update()
    {
        TargetRotation = Vector3.Lerp(TargetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        CurrRotation = Vector3.Slerp(CurrRotation, TargetRotation, Snappines * Time.deltaTime);
        AffectedObject.transform.localRotation = Quaternion.Euler(CurrRotation);
    }

    public void RecoilFire(Vector3 Recoil)
    {
        Recoil = Recoil * RecoilMultipiler;
        TargetRotation -= new Vector3(Recoil.y, Recoil.x,Recoil.z);
    }
}