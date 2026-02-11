using System.Collections;
using UnityEngine;

public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] float Force;

    Rigidbody rb;

    [SerializeField] FlashNade flashNade;

    [SerializeField] SmokeNade smokeNade;


    private void Update()
    {
        
    }


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
