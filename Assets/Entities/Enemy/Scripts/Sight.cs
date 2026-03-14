using System;
using Unity.Collections;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] float ForwardMax;
    [SerializeField] float Angle;
    [SerializeField] float ForwardMin;
    [SerializeField] float UpMin;
    [SerializeField] float UpMax;
    [SerializeField] GameObject Target;

    [SerializeField] int ChecksPerSecond = 10;
    
    [Header("dots")]
    public float ForwardDot;
    public float RightDot;
    public float UpDot;

    [Header("angles")]
    public float forwardCos;
    public float rightCos;
    public float upCos;

    [Header("Awarness Parameter")]
    public float MaxAwareness;
    public float currentAwareness=0f;
    public float AwarenessSpeed;

    public event EventHandler OnTargetinSIght;
    public event EventHandler OnTargetoutSIght;

    private bool inSight;
    private bool inCone;
    private bool previousInSight;
    private float timer;

    bool CheckUpdate()
    {
        timer += Time.deltaTime;
        float interval = ChecksPerSecond > 0 ? 1f / ChecksPerSecond : float.MaxValue;

        if (timer < interval)
            return false;
        else
            timer = 0f;
            return true;
    }

    private void Update()
    {
        if (!CheckUpdate())
        {
            return;
        }


        if (inSight)
        {
            if (currentAwareness < MaxAwareness)
                currentAwareness += AwarenessSpeed * Time.deltaTime;
        }
        else
        {
            if (currentAwareness > 0)
                currentAwareness -= AwarenessSpeed * Time.deltaTime;
        }

        Vector3 direction = (Target.transform.position - transform.position).normalized;

        // dot product calculation
        ForwardDot = Vector3.Dot(direction, transform.forward);
        RightDot = Vector3.Dot(direction, transform.right);
        UpDot = Vector3.Dot(direction, transform.up);

        // angle calculation
        forwardCos = Mathf.Acos(ForwardDot) * Mathf.Rad2Deg;
        rightCos = Mathf.Acos(RightDot) * Mathf.Rad2Deg;
        upCos = Mathf.Acos(UpDot) * Mathf.Rad2Deg;

        // cone checks if it inside cone
        inCone = (rightCos > Angle && rightCos < Angle + 90f)
                && (ForwardDot > ForwardMin)
                && (Target.transform.position - transform.position).magnitude < ForwardMax
                && (upCos > UpMin && upCos < UpMax);

        if (inCone)
        {
            // sight is when the raycast checks for obstacle which can block the sight
            float distance = (Target.transform.position - transform.position).magnitude;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
                inSight = hit.collider.gameObject == Target;
            else
                inSight = false;
        }
        else
        {
            inSight = false;
        }

        if (inSight != previousInSight)
        {
            if (inSight && currentAwareness>MaxAwareness*0.95f)
                OnTargetinSIght?.Invoke(this, EventArgs.Empty);
            else if(currentAwareness<0.01f)
                OnTargetoutSIght?.Invoke(this, EventArgs.Empty);

            previousInSight = inSight;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float halfCone = 90f - Angle;
        Gizmos.color = inSight ? Color.green : Color.red;

        Vector3 leftDir = Quaternion.AngleAxis(-halfCone, transform.up) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(halfCone, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * ForwardMax);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * ForwardMax);

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