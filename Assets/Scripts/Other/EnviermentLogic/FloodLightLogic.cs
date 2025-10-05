using UnityEngine;
public class FloodLightLogic : MonoBehaviour
{
    public enum MoveDirection { x, y }
    public MoveDirection direction;

    [SerializeField] float Speed;

    [SerializeField] float MaxDegree;
    [SerializeField] float MinDegree;

    [SerializeField] GameObject LightSource;

    [SerializeField] Vector3 ro;
    [SerializeField] float LightRotateDirection;

    private void Start()
    {
        ro = transform.eulerAngles;
    }
    private void Update()
    {
        if(direction == MoveDirection.x)
        {
            if(ro.x >= MaxDegree )
            {
                LightRotateDirection= -1f;
            }
            if (ro.x <= MinDegree)
            {
                LightRotateDirection = 1f;
            }
            ro.x += Speed * Time.deltaTime* LightRotateDirection;

        }
        else
        {
            if (ro.y >= MaxDegree)
            {
                LightRotateDirection = -1f;
            }
            if (ro.y <= MinDegree)
            {
                LightRotateDirection = 1f;
            }
            ro.y += Speed * Time.deltaTime * LightRotateDirection;

        }
        gameObject.transform.eulerAngles = ro;


        DrawWireCapsule(CapsuleStart,CapsuleEnd,Radius,Color.black);
    }

    [SerializeField] Vector3 CapsuleStart;
    [SerializeField] Vector3 CapsuleEnd;
    [SerializeField] float Radius;

    void CheckEnemy()
    {
        Physics.CheckCapsule(CapsuleStart, CapsuleEnd, Radius);
    }



    public static void DrawWireCapsule(Vector3 start, Vector3 end, float radius, Color color)
    {
        Gizmos.color = color;

        // Draw end spheres
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);

        // Direction between spheres
        Vector3 up = (end - start).normalized;
        Vector3 right = Vector3.Cross(up, Vector3.up);
        if (right == Vector3.zero) // handle degenerate case
            right = Vector3.Cross(up, Vector3.forward);
        right.Normalize();
        Vector3 forward = Vector3.Cross(up, right);

        // Four lines connecting spheres
        Gizmos.DrawLine(start + right * radius, end + right * radius);
        Gizmos.DrawLine(start - right * radius, end - right * radius);
        Gizmos.DrawLine(start + forward * radius, end + forward * radius);
        Gizmos.DrawLine(start - forward * radius, end - forward * radius);
    }
    


}
