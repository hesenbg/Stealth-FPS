using System;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] float ForwardMax;
    [SerializeField] float Angle;
    [SerializeField] float ForwardMin;

    [SerializeField] float UpMin;
    [SerializeField] float UpMax;
    [SerializeField] GameObject Target;

    [Header("dots")]
    public float ForwardDot;
    public float RightDot;
    public float UpDot;

    [Header("angles")]
    public float forwardCos;
    public float rightCos;
    public float upCos;

    public event EventHandler OnTargetinSIght;
    public event EventHandler OnTargetoutSIght;

    private bool inSight;

    private void FixedUpdate()
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;

        ForwardDot = Vector3.Dot(direction, transform.forward);
        RightDot = Vector3.Dot(direction, transform.right);
        UpDot = Vector3.Dot(direction, transform.up);

        forwardCos = Mathf.Acos(ForwardDot) * Mathf.Rad2Deg;
        rightCos = Mathf.Acos(RightDot) * Mathf.Rad2Deg;
        upCos = Mathf.Acos(UpDot) * Mathf.Rad2Deg;

        inSight =  (rightCos > Angle && rightCos < Angle + 90f)
                && (ForwardDot > ForwardMin)
                && (Target.transform.position - transform.position).magnitude < ForwardMax
                && (upCos > UpMin && upCos < UpMax);
                

        if (inSight)
            OnTargetinSIght?.Invoke(this, EventArgs.Empty);
        else
            OnTargetoutSIght?.Invoke(this, EventArgs.Empty);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float halfCone = 90f - Angle;
        Gizmos.color = inSight ? Color.green : Color.red;

        // lines
        Vector3 leftDir = Quaternion.AngleAxis(-halfCone, transform.up) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(halfCone, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * ForwardMax);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * ForwardMax);

        // arc
        int steps = 8;
        Vector3 prev = Vector3.zero;
        for (int i = 0; i <= steps; i++)
        {
            float a = Mathf.Lerp(-halfCone, halfCone, (float)i / steps);
            Vector3 p = transform.position + (Quaternion.AngleAxis(a, transform.up) * transform.forward) * ForwardMax;
            if (i > 0) Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }
#endif
}