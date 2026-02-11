using UnityEngine;
using UnityEngine.UIElements;

public class LaserDot : MonoBehaviour
{
    [SerializeField] float Lenght;
    LineRenderer Laser;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Laser.SetPosition(0,Laser.transform.position);
        RaycastHit LaserHit;
        if(Physics.Raycast(transform.position,transform.forward, out LaserHit, Lenght))
        {
            Laser.SetPosition(1, LaserHit.point);
        }
        else
        {
            Laser.SetPosition(1, transform.position + transform.forward.normalized * Lenght);
        }
    }
}
