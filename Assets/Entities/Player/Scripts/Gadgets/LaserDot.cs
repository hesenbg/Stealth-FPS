using UnityEngine;

public class LaserDot : MonoBehaviour
{
    [SerializeField] float Length = 50f;
    LineRenderer Laser;

    void Start()
    {
        Laser = GetComponent<LineRenderer>();
        Laser.useWorldSpace = false;
    }

    void LateUpdate()
    {
        Laser.SetPosition(0, Vector3.zero);

        RaycastHit LaserHit;
        if (Physics.Raycast(transform.position, transform.forward, out LaserHit, Length))
        {
            Vector3 localHitPoint = transform.InverseTransformPoint(LaserHit.point);
            Laser.SetPosition(1, localHitPoint);
        }
        else
        {
            Laser.SetPosition(1, Vector3.forward * Length);
        }
    }
}