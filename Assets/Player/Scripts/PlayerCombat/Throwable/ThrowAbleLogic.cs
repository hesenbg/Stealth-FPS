using System.Collections;
using UnityEngine;

public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] float Force;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Release()
    {
        Transform cam = PlayerComponents.Instance.MainCamera.transform;
        rb.AddForce((cam.forward + cam.up) * Force);
    }
}
