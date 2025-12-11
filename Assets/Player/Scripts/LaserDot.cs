using UnityEngine;

public class LaserDot : MonoBehaviour
{

    LineRenderer Laser;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Laser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit LaserHit;
        if(Physics.Raycast(transform.position,transform.forward, out LaserHit))
        {
            Laser.SetPosition(1, LaserHit.point);
        }
    }
}
