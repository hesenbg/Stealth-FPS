using System.Collections.Generic;
using UnityEngine;
public  class LightCone : MonoBehaviour
{
    [SerializeField] protected float VerticalAngle;

    [SerializeField] protected float HorizontalAngle;

    [SerializeField] protected float Range;

    [SerializeField] float AngleAdjustLight;

    [SerializeField] float LightObservability = 1f;

    [SerializeField] LayerMask TargetMask;

    [SerializeField] LayerMask ObstacleMask;

    [SerializeField] protected List<Transform> targets;

    Vector3 flatTarget;

    Vector3 verticalTarget;

    Light SpotLight;

    private void Start()
    {
        SpotLight = GetComponent<Light>();
    }

    private void Update()
    {
        targets = GetObjectsInCone();

        UpdateObservability();

        if (SpotLight != null)
        {
            SpotLight.range = Range;

            SpotLight.spotAngle = HorizontalAngle * 2f + AngleAdjustLight;
        }

    }

    private HashSet<Transform> previousTargets = new HashSet<Transform>();
    private HashSet<Transform> newThisFrame = new HashSet<Transform>();

    void UpdateObservability()
    {
        HashSet<Transform> currentTargets = new HashSet<Transform>(targets);
        newThisFrame.Clear();

        foreach (Transform t in currentTargets)
        {
            if (!previousTargets.Contains(t))
                newThisFrame.Add(t);
        }

        foreach (Transform t in previousTargets)
        {
            if (!currentTargets.Contains(t) && t != null && !newThisFrame.Contains(t))
            {
                if (t.TryGetComponent<ObservableObject>(out ObservableObject obs))
                    obs.RemoveModifier(t.name);
            }
        }

        foreach (Transform t in currentTargets)
        {
            if (t.TryGetComponent<ObservableObject>(out ObservableObject obs))
                obs.AddModifier(t.name, LightObservability);
        }

        previousTargets = currentTargets;
    }

    public List<Transform> GetObjectsInCone()
    {
        List<Transform> Objects = new List<Transform>();

        Collider[] Targets = Physics.OverlapSphere(transform.position, Range, TargetMask, QueryTriggerInteraction.Ignore);

        foreach (Collider collider in Targets)
        {
            if (IsInCone(collider.transform) )
            {
                if (!Physics.Raycast(transform.position, (collider.transform.position - transform.position).normalized, out RaycastHit hit, Range, ObstacleMask, QueryTriggerInteraction.Collide))
                {
                    Objects.Add(collider.transform);
                }
            }
        }
        return Objects;
    }

    private bool IsInCone(Transform Target)
    {
        Vector3 toTarget = Target.position - transform.position;

        flatTarget = Vector3.ProjectOnPlane(toTarget, transform.up).normalized;

        float horizontalDot = Vector3.Dot(transform.forward, flatTarget);

        float horizontalAngle = Mathf.Acos(Mathf.Clamp(horizontalDot, -1f, 1f)) * Mathf.Rad2Deg;


        verticalTarget = Vector3.ProjectOnPlane(toTarget, transform.right).normalized;

        float verticalDot = Vector3.Dot(transform.forward, verticalTarget);

        float verticalAngle = Mathf.Acos(Mathf.Clamp(verticalDot, -1f, 1f)) * Mathf.Rad2Deg;

        return horizontalAngle <= HorizontalAngle && verticalAngle <= VerticalAngle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 leftDir = Quaternion.AngleAxis(-VerticalAngle, transform.right) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(VerticalAngle, transform.right) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * Range);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * Range);

        // vertical arc
        int steps = 20;
        for (int i = 0; i < steps; i++)
        {
            float t1 = Mathf.Lerp(-VerticalAngle, VerticalAngle, (float)i / steps);
            float t2 = Mathf.Lerp(-VerticalAngle, VerticalAngle, (float)(i + 1) / steps);
            Vector3 p1 = transform.position + (Quaternion.AngleAxis(t1, transform.right) * transform.forward) * Range;
            Vector3 p2 = transform.position + (Quaternion.AngleAxis(t2, transform.right) * transform.forward) * Range;
            Gizmos.DrawLine(p1, p2);
        }

        leftDir = Quaternion.AngleAxis(-HorizontalAngle, transform.up) * transform.forward;
        rightDir = Quaternion.AngleAxis(HorizontalAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * Range);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * Range);

        // horizontal arc
        for (int i = 0; i < steps; i++)
        {
            float t1 = Mathf.Lerp(-HorizontalAngle, HorizontalAngle, (float)i / steps);
            float t2 = Mathf.Lerp(-HorizontalAngle, HorizontalAngle, (float)(i + 1) / steps);
            Vector3 p1 = transform.position + (Quaternion.AngleAxis(t1, transform.up) * transform.forward) * Range;
            Vector3 p2 = transform.position + (Quaternion.AngleAxis(t2, transform.up) * transform.forward) * Range;
            Gizmos.DrawLine(p1, p2);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + flatTarget);
        Gizmos.color = Color.orange;
        Gizmos.DrawLine(transform.position, transform.position + verticalTarget);
    }
}
