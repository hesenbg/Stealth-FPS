using System.Collections;
using UnityEngine;
using static BaseAI;


public class Alarm : MonoBehaviour
{
    BarricadePositions Closest;
    EnemyAnimationLogic Animation;
    BaseAI Base;
    public EnemyType OurEnemyType;
    public enum AlarmStates { Fighting, Defend, Null }
    public AlarmStates CurrentAlarmState;

    // variables
    Collider[] Barricades;
    float DistanceFromBarrier;
    public bool HasTakenPosition;
    [SerializeField] Vector3 HalfExtend;
    Vector3 ClosestBarricadePos;
    [HideInInspector] public bool IsRightBarricadeSide;

    private void Start()
    {
        Base = GetComponent<BaseAI>();
        Animation  = GetComponentInChildren<EnemyAnimationLogic>();
        Closest = null;
        CurrentAlarmState = AlarmStates.Fighting;
    }
    public void UpdateAlarm() // based on the enemy type,
    {
        if (Base.Sight.TargetOnSight)
        {
            CurrentAlarmState = AlarmStates.Fighting;
        }
        else
        {
            CurrentAlarmState = AlarmStates.Defend;
        }

        Base.UpdateRotation(Base.PlayerSpotPosition, 5f);

        switch (CurrentAlarmState)
        {
            case AlarmStates.Fighting:
                Fight();
                break;
            case AlarmStates.Defend:
                Defence();
                break;
        }
    }

    void Fight()
    {
        Shoot();
    }

    void Defence()
    {

        Debug.Log(LeanDurationValue);
        if(OurEnemyType == EnemyType.defender)
        {
            TakePosition();
            if (HasTakenPosition)
            {
                Lean();
            }
        }
        else
        {
            Base.Tracktarget(Base.PlayerSpotPosition);
        }
    }

    void Shoot()
    {
        Debug.Log("shoot");
    }

    BarricadePositions FindClosest()
    {
        // get the colliders of close barricades
        Barricades = Physics.OverlapBox(
            transform.position,
            HalfExtend,
            Quaternion.identity,
            LayerMask.GetMask("Barricade")
        );

        // find the closest one 
        foreach (Collider c in Barricades)
        {
            float dist = Vector3.Distance(c.transform.position, transform.position);
            if (dist < DistanceFromBarrier)
            {
                DistanceFromBarrier = dist;
                Closest = c.GetComponent<BarricadePositions>();
            }
        }
        IsRightBarricadeSide = Closest.IsRight;

        return Closest;
    }
    bool HasFindPosition = false;
    void TakePosition()  // sreach up a barricade, 
    {
        if (!HasFindPosition)
        {
            DistanceFromBarrier = Mathf.Infinity;

            Closest = FindClosest();

            if(Closest == null)
            {
                return;
            }
        }

        HasFindPosition = true;
        ClosestBarricadePos = Closest.FindClosestPosition(transform.position);

        if (Base.Tracktarget(ClosestBarricadePos))
        {
            HasTakenPosition = true;
        }
    }

    public bool IsLeaning = false;

    [SerializeField] float LeanDuration;
    float LeanDurationValue = 0;

    void Lean()
    {
        if (LeanDurationValue < LeanDuration)
        {
            IsLeaning = false;
            LeanDurationValue += Time.deltaTime;
        }
        else
        {
            IsLeaning = true;
            LeanDurationValue = 0;
            if (IsRightBarricadeSide)
            {
                Animation.LeanRight();
            }
            else
            {
                Animation.LeanLeft();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, HalfExtend * 2);
    }

}