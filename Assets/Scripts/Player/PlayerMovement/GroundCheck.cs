using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsPressingGround= false;

    BoxCollider GroundTrigger;
    [SerializeField] PlayerMovement PlayerMovement;
    Vector3 BaseCenter;
    private void Start()
    {
        GroundTrigger = GetComponent<BoxCollider>();
        BaseCenter = GroundTrigger.center;
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.IsCrouching)
        {
            GroundTrigger.center = new Vector3(0, -0.5f, 0);
        }
        else
        {
            GroundTrigger.center = BaseCenter;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            IsPressingGround = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            IsPressingGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            IsPressingGround = false;
        }
    }
}
