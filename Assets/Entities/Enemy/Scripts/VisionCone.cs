using System;
using System.Collections;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    #region Configuration
    [SerializeField] float Range;
    [SerializeField] float ForwardMin;
    [SerializeField] float ForwardMax;
    [SerializeField] float IndicatorAngleDiff;
    [SerializeField] LayerMask VisionMask;
    [SerializeField] int ChecksPerSecond = 10;
    #endregion

    #region Awareness Settings
    [Header("Awareness")]
    public float AlarmAwareness;
    public float SuspiciousAwareness;
    public float AwarenessSpeed;
    public float currentAwareness;
    #endregion

    #region Debug & Data
    [Header("Debug")]
    public float forwardCos;

    public EnemyAIData data;
    #endregion

    #region Events
    public event EventHandler<EventData> TargetFullySeen;
    public event EventHandler<EventData> TargetAnomalySeen;
    public event EventHandler<EventData> TargetSuspiciousSight;
    private event EventHandler<EventData> TargetoutSight;
    private event EventHandler<EventData> TargetEnterSight;
    #endregion

    #region Private State
    private bool inSight;
    public bool inCone;
    private bool suspiciousFired;
    private bool alarmFired;
    private float timer;
    private Indicator SightIndicator;
    private ObservableObject MainTargetedObject;
    private ObservableObject previousTarget;
    private float smoothedAngle;
    private enum TargetType { Player, DeadBody }
    #endregion

    private void Awake()
    {
        TargetEnterSight += OnTargetEnterSight;
        TargetoutSight += OnTargetoutSight;
    }

    private void Start()
    {
        data = GetComponentInParent<EnemyStateMachine>().context.enemyAIData;
    }

    private void Update()
    {
        UpdateUI();
        UpdateAwareness();
        if (!CheckUpdate()) return;
        UpdateLogic();
    }

    public void RotateSight() => StartCoroutine(RotateSightRoutine());

    public void StopRotateSight()
    {
        StopCoroutine(RotateSightRoutine());
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    IEnumerator RotateSightRoutine()
    {
        float[] stops = { data.current.AroundCheckAngle, -data.current.AroundCheckAngle, 0f };
        foreach (float target in stops)
        {
            while (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.y, target)) > 0.1f)
            {
                float current = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, target, data.current.AroundCheckSpeed * Time.deltaTime);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, current, transform.localEulerAngles.z);
                yield return null;
            }
            yield return new WaitForSeconds(data.current.AroundCheckDelay);
        }
    }

    bool CheckUpdate()
    {
        timer += Time.deltaTime;
        float interval = ChecksPerSecond > 0 ? 1f / ChecksPerSecond : float.MaxValue;
        if (timer < interval) return false;
        timer = 0f;
        return true;
    }

    Collider[] Targets;

    void UpdateLogic()
    {
        Targets = Physics.OverlapSphere(transform.position, Range, VisionMask);

        ObservableObject highestPriority = null;

        foreach (Collider target in Targets)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            forwardCos = MathFunc.ForwardSight(direction, transform.forward);
            inCone = forwardCos > ForwardMin && forwardCos < ForwardMax;

            if (!inCone) continue;
            if (!CheckInSight(target.gameObject)) continue;
            if (!target.TryGetComponent<ObservableObject>(out ObservableObject observable)) continue;
            if (observable.Observability == 0) continue;
            if(observable.HasSeen) continue;

            if (highestPriority == null || observable.Priority < highestPriority.Priority)
                highestPriority = observable;
        }
        MainTargetedObject = highestPriority;

        inSight = MainTargetedObject != null;
    }

    bool CheckInSight(GameObject InSightObject)
    {
        Vector3 direction = (InSightObject.transform.position - transform.position).normalized;
        if (inCone && Physics.Raycast(transform.position, direction, out RaycastHit hit, Range, VisionMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject == InSightObject)
                return true;
        }
        return false;
    }

    void UpdateAwareness()
    {
        float prev = currentAwareness;

        if (inSight)
        {
            if (currentAwareness < AlarmAwareness)
                currentAwareness += AwarenessSpeed * MainTargetedObject.Observability * Time.deltaTime;
        }
        else
        {
            if (currentAwareness > 0f)
                currentAwareness -= AwarenessSpeed * Time.deltaTime;
        }

        currentAwareness = Mathf.Clamp(currentAwareness, 0f, AlarmAwareness);

        if (inSight)
        {
            Vector3 direction = (MainTargetedObject.Transform.position - transform.position).normalized;
            EventData sightData = new EventData(MainTargetedObject.Transform.position, direction);

            if (prev == 0f)
                TargetEnterSight?.Invoke(this, sightData);

            if (!suspiciousFired && currentAwareness >= SuspiciousAwareness)
            {
                suspiciousFired = true;
                TargetSuspiciousSight?.Invoke(this, sightData);
            }

            if (currentAwareness >= AlarmAwareness)
            {
                currentAwareness = 0f;
                MainTargetedObject.SetHasSeen();
                if (MainTargetedObject.Type == ObservableType.Hostile)
                    TargetFullySeen?.Invoke(this, sightData);
                else if (MainTargetedObject.Type == ObservableType.Clue)
                    TargetAnomalySeen?.Invoke(this, sightData);

                MainTargetedObject = null;
                inSight = false;
            }
        }

        if (!inSight)
        {
            if (suspiciousFired && currentAwareness < SuspiciousAwareness)
                suspiciousFired = false;

            if (alarmFired && currentAwareness < AlarmAwareness)
                alarmFired = false;

            if (prev > 0f && currentAwareness <= 0f)
                TargetoutSight?.Invoke(this, new EventData());
        }

        if (MainTargetedObject != previousTarget)
        {
            bool wasHostile = previousTarget?.Type == ObservableType.Hostile;
            bool isHostile = MainTargetedObject?.Type == ObservableType.Hostile;

            if (wasHostile && !isHostile)
            {
                if (SightIndicator != null) { Destroy(SightIndicator.parent); SightIndicator = null; }
            }
            else if (!wasHostile && isHostile)
            {
                if (SightIndicator != null) Destroy(SightIndicator.parent);
                SightIndicator = PlayerComponents.Instance.PlayerUI.CreateIndicator();
            }

            previousTarget = MainTargetedObject;
        }
    }

    void UpdateUI()
    {
        if (SightIndicator == null) return;

        Vector3 dir = (transform.position - PlayerComponents.Instance.Player.transform.position).normalized;
        float targetAngle = Vector3.SignedAngle(dir, PlayerComponents.Instance.Player.transform.forward, Vector3.up) - IndicatorAngleDiff;

        smoothedAngle = Mathf.LerpAngle(smoothedAngle, targetAngle, Time.deltaTime * 20f);

        SightIndicator.UpdateIndicator(currentAwareness, AlarmAwareness, smoothedAngle);
    }

    private void OnTargetEnterSight(object sender, EventArgs e)
    {
        if (MainTargetedObject?.Type != ObservableType.Hostile) return;
        if (SightIndicator != null) Destroy(SightIndicator.parent);
        SightIndicator = PlayerComponents.Instance.PlayerUI.CreateIndicator();
    }

    private void OnTargetoutSight(object sender, EventArgs e)
    {
        if (SightIndicator == null) return;
        Destroy(SightIndicator.parent);
    }

    private void OnDestroy()
    {
        if (SightIndicator != null) Destroy(SightIndicator.parent);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float halfCone = 90f - ForwardMax;
        Gizmos.color = inSight ? Color.green : Color.red;

        Vector3 leftDir = Quaternion.AngleAxis(-halfCone, transform.up) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(halfCone, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * Range);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * Range);

        int steps = 8;
        Vector3 prev = Vector3.zero;
        for (int i = 0; i <= steps; i++)
        {
            float a = Mathf.Lerp(-halfCone, halfCone, (float)i / steps);
            Vector3 p = transform.position + (Quaternion.AngleAxis(a, transform.up) * transform.forward) * Range;
            if (i > 0) Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }
#endif
}