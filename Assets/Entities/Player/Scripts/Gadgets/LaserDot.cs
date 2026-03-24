using UnityEngine;

public class LaserDot : MonoBehaviour
{
    [SerializeField] float Length = 50f;
    LineRenderer Laser;
    [SerializeField] GameObject LaserQuad;
    [SerializeField] float surfaceOffset = 0.02f;
    GameObject quad;
    
    void Start()
    {
        quad= Instantiate(LaserQuad, transform.position, transform.rotation);
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

            quad.SetActive(true);

            quad.transform.position = LaserHit.point + (LaserHit.normal * surfaceOffset);
            quad.transform.rotation = Quaternion.LookRotation(-LaserHit.normal);
        }
        else
        {
            Laser.SetPosition(1, Vector3.forward * Length);
            quad.SetActive(false);
        }
    }
}