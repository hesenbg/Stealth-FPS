using System.Linq;
using System.Threading;
using Unity.AppUI.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;
public class FloodLightLogic : MonoBehaviour
{
    public enum MoveDirection { x, y }
    public MoveDirection direction;
    [SerializeField] Vector3[] rays;

    [SerializeField] float Speed;

    [SerializeField] float MaxDegree;
    [SerializeField] float MinDegree;

    [SerializeField] Vector3 ro;
    [SerializeField] float LightRotateDirection;

    [SerializeField] Light spotLight;

    GameObject Player;

    private void Start()
    {
        ro = transform.eulerAngles;
        Player = GameObject.Find("Player");
    }
    private void Update()
    {
        MoveLight();
        CheckEnemy();
    }

    void MoveLight()
    {
        if (direction == MoveDirection.x)
        {
            if (ro.x >= MaxDegree)
            {
                LightRotateDirection = -1f;
            }
            if (ro.x <= MinDegree)
            {
                LightRotateDirection = 1f;
            }
            ro.x += Speed * Time.deltaTime * LightRotateDirection;

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
    }



    [SerializeField] float Range;

    void CheckEnemy()
    {
        Vector3 pos = spotLight.transform.position;
        Vector3 dir = spotLight.transform.forward;
        float angle = spotLight.spotAngle * 0.5f;
        float range = spotLight.range;

        float radius = Mathf.Tan(angle * Mathf.Deg2Rad) * range;
        Quaternion rot;

        for (int i = 0; i < rays.Length; i++)
        {
            rot = Quaternion.AngleAxis(angle, transform.TransformDirection(rays[i]));
            Vector3 right = rot * dir;

            if(Physics.Raycast(pos, right*range, out RaycastHit hit))
            {
                if(hit.collider.gameObject.layer == 3)
                {
                    Debug.Log("player seen");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = spotLight.transform.position;
        Vector3 dir = spotLight.transform.forward;
        float angle = spotLight.spotAngle * 0.5f;
        float range = spotLight.range;

        float radius = Mathf.Tan(angle * Mathf.Deg2Rad) * range;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(pos, dir * range);

        Quaternion rot;
        for (int i = 0; i < rays.Length; i++)
        {
            rot = Quaternion.AngleAxis(angle, transform.TransformDirection(rays[i]));
            Vector3 right = rot * dir;
            Gizmos.DrawRay(pos, right * range);
        }
    }
}
