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

    Vector3 direction;

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
    public float AlarmAwareness;
    public float SuspiciousAwareness;
    public float currentAwareness = 0f;
    public float AwarenessSpeed;

    public event EventHandler TargetSuspiciousSight;
    public event EventHandler TargetoutSight;
    public event EventHandler TargetFullySeen;
    public event EventHandler TargetEnterSight;

    private bool inSight;
    private bool inCone;
    private float timer;
    private bool suspiciousFired;
    private bool alarmFired;

    private void Awake()
    {
        TargetEnterSight += OnTargetEnterSight;
        TargetoutSight += OnTargetoutSight;
    }

    bool CheckUpdate()
    {
        timer += Time.deltaTime;
        float interval = ChecksPerSecond > 0 ? 1f / ChecksPerSecond : float.MaxValue;
        if (timer < interval) return false;
        timer = 0f;
        return true;
    }

    void UpdateAwareness()
    {
        float prev = currentAwareness;

        if (inSight)
        {
            if (currentAwareness < AlarmAwareness)
                currentAwareness += AwarenessSpeed * Time.deltaTime;
        }
        else
        {
            if (currentAwareness > 0f)
                currentAwareness -= AwarenessSpeed * Time.deltaTime;
        }

        currentAwareness = Mathf.Clamp(currentAwareness, 0f, AlarmAwareness);

        if (!suspiciousFired && currentAwareness >= SuspiciousAwareness)
        {
            suspiciousFired = true;
            TargetSuspiciousSight?.Invoke(this, EventArgs.Empty);
        }
        else if (suspiciousFired && currentAwareness < SuspiciousAwareness)
        {
            suspiciousFired = false;
        }

        if (!alarmFired && currentAwareness >= AlarmAwareness)
        {
            alarmFired = true;
            TargetFullySeen?.Invoke(this, EventArgs.Empty);
        }
        else if (alarmFired && currentAwareness < AlarmAwareness)
        {
            alarmFired = false;
        }

        if (prev > 0f && currentAwareness <= 0f)
            TargetoutSight?.Invoke(this, EventArgs.Empty);
    }

    void UpdateLogic()
    {
        direction = (Target.transform.position - transform.position).normalized;

        forwardCos = MathFunc.ForwardSight(direction, transform.forward);
        rightCos = MathFunc.RightSight(direction, transform.right);
        upCos = MathFunc.UpSight(direction, transform.up); // was transform.right

        inCone = (rightCos > Angle && rightCos < Angle + 90f)
                && (forwardCos > ForwardMin) // was ForwardDot (never assigned)
                && (Target.transform.position - transform.position).magnitude < ForwardMax
                && (upCos > UpMin && upCos < UpMax);
    }

    void CheckInSight()
    {
        bool newInSight = false;

        if (inCone)
        {
            float distance = (Target.transform.position - transform.position).magnitude;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
                newInSight = hit.collider.gameObject == Target;
        }

        if (newInSight && !inSight)
            TargetEnterSight?.Invoke(this, EventArgs.Empty);

        inSight = newInSight;
    }

    private void OnTargetoutSight(object sender, EventArgs e)
    {
        Destroy(SightIndicator);
    }

    private void OnTargetEnterSight(object sender, EventArgs e)
    {
        SightIndicator = PlayerComponents.Instance.PlayerUI.CreateIndicator();
    }

    private Indicator SightIndicator;

    void UpdateUI()
    {
        if (SightIndicator == null) return;
        SightIndicator.UpdateIndicator(currentAwareness, AlarmAwareness);
    }

    private void Update()
    {
        UpdateUI();
        UpdateAwareness();

        if (!CheckUpdate()) return;

        UpdateLogic();
        CheckInSight();
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