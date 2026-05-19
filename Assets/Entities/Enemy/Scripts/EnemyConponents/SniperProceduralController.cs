using UnityEngine;

public class SniperProceduralController : MonoBehaviour
{
    [SerializeField] Transform GunRigController;

    [SerializeField] Transform target;

    private Quaternion _initialRotation;

    private void Awake()
    {
        _initialRotation = GunRigController.localRotation;
    }

    public bool UpdateRigRotation(Vector3 Direction, float Speed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Direction);
        GunRigController.rotation = Quaternion.Slerp(GunRigController.rotation, targetRotation, Speed * Time.deltaTime);
        return Quaternion.Angle(GunRigController.rotation, targetRotation) < 1f;
    }


    private void Update()
    {
        //UpdateRigRotation(target.position - GunRigController.position,5f);
    }
}