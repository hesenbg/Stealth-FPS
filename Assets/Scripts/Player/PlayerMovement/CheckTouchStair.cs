using UnityEngine;

public class CheckTouchStair : MonoBehaviour
{
    public bool IsTouchingStair= false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            IsTouchingStair = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            IsTouchingStair = false;
        }
    }
}
