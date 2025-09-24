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
    }
    public void UpdateAlarm()
    {
        switch (CurrentAlarmState)
        {
            case AlarmStates.Fighting:
                Fight();
                break;
            case AlarmStates.Defend:
                Defend();
                break;
        }
    }

    void Fight()
    {
        Base.Tracktarget(Base.PlayerSpotPosition);
    }
    void Defend()
    {
        TakePosition();
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
    bool TakePosition()  // sreach up a barricade, 
    {
        DistanceFromBarrier = Mathf.Infinity;

        Closest = FindClosest();

        if(Closest == null)
        {
            return false;
        }


        if( Closest != null)
        {
            ClosestBarricadePos = Closest.FindClosestPosition(transform.position);
        }

        if (Base.Tracktarget(ClosestBarricadePos))
        {
            HasTakenPosition = true;
        }
        return true;
    }

}