using UnityEngine;

public class FloodLightLogic : MonoBehaviour
{
    public enum MoveDirection { x, y }
    public MoveDirection direction;
    [SerializeField] Vector3[] rays;
    [SerializeField] float[] Angles;

    [SerializeField] float Speed;

    [SerializeField] float MaxDegree;
    [SerializeField] float MinDegree;

    [SerializeField] Vector3 currentRotation; // Renamed for clarity
    [SerializeField] float LightRotateDirection;

    [SerializeField] Light spotLight;
    [SerializeField] float Range;
    Vector3 pos;
    Vector3 dir;
    float angle;
    float range;

    GameObject Player;

    private void Start()
    {
        currentRotation = transform.localEulerAngles;
        Player = GameObject.Find("Player");

        LightRotateDirection = 1;

        pos = spotLight.transform.position;
        dir = spotLight.transform.forward;
        angle = spotLight.spotAngle;
        range = spotLight.range;
    }

    private void FixedUpdate()
    {
        MoveLight();
        CheckEnemy();
    }

    void MoveLight()
    {
        float currentAngle = GetCurrentAngle();

        if (currentAngle >= MaxDegree)
        {
            LightRotateDirection = -1f;
        }
        else if (currentAngle <= MinDegree)
        {
            LightRotateDirection = 1f;
        }

        float rotationThisFrame = Speed * Time.deltaTime * LightRotateDirection;

        if (direction == MoveDirection.x)
        {
            currentRotation.x += rotationThisFrame;
        }
        else
        {
            currentRotation.y += rotationThisFrame;
        }

        transform.localEulerAngles = currentRotation;
    }

    // euler degrees are in range of 0 360
    float GetCurrentAngle()
    {
        float angle = direction == MoveDirection.x ? currentRotation.x : currentRotation.y;

        if (angle > 180f)
            angle -= 360f;

        return angle;
    }

    void CheckEnemy()
    {
        pos = spotLight.transform.position;
        dir = spotLight.transform.forward;
        angle = spotLight.spotAngle;
        range = spotLight.range;

        for (int j = 0; j < Angles.Length; j++)
        {
            for (int i = 0; i < rays.Length; i++)
            {
                float currentAngle = Angles[j];
                Quaternion rot = Quaternion.AngleAxis(currentAngle, transform.TransformDirection(rays[i]));
                Vector3 right = rot * dir;

                if (Physics.Raycast(pos, right, out RaycastHit hit, range))
                {
                    if (hit.collider.gameObject.layer == 3) // Make sure layer 3 is your player layer
                    {
                        Debug.Log("player seen");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(pos, dir * range);

        for (int j = 0; j < Angles.Length; j++)
        {
            for (int i = 0; i < rays.Length; i++)
            {
                float currentAngle = Angles[j];
                Quaternion rot = Quaternion.AngleAxis(currentAngle, transform.TransformDirection(rays[i]));
                Vector3 right = rot * dir;

                Gizmos.DrawRay(pos, right * range);
            }
        }
    }
}