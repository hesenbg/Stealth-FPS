using UnityEngine;
using static BaseAI;


public class Alarm : MonoBehaviour
{
    BarricadePositions Closest;
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

    private void Start()
    {
        Base = GetComponent<BaseAI>();
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
        Base.UpdateRotation(Base.PlayerSpotPosition, 5f);
        Shoot();
    }

    void Defence()
    {
        if(OurEnemyType == EnemyType.defender)
        {
            TakePosition();
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

}