using UnityEngine;
public class BulletLogic : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] CameraPowLogic Origin;

    [SerializeField] float Velocity;

    [SerializeField] Vector3 Direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();   
    }
}