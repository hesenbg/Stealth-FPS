using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Suspicious : MonoBehaviour
{
    BaseAI Base;
    float RotationSpeed;

    public bool IsInvestigates;

    [SerializeField] float InvestigationTime;
    float InvestigationTimeValue;

    private void Start()
    {
        Base = GetComponent<BaseAI>();
    }

    public void UpdateSuspicious() // checks position and goes back 
    {
        UpdateRotation(Base.TriggerPosition);
        if (Base.Tracktarget(Base.TriggerPosition))
        {
            if (InvestigationTimeValue < InvestigationTime)
            {
                InvestigationTimeValue += Time.deltaTime;
            }
            else
            {
                InvestigationTimeValue = 0;
                Base.IsEnemyDistracted = false;
            }
        }
    }

    void UpdateRotation(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f) // checks if position has reached(not exact position)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion offsetRotation = Quaternion.Euler(0, 0, 0); // meaningless but looks cool(start point)
            Quaternion finalRotation = targetRotation * offsetRotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                finalRotation,
                Time.deltaTime * RotationSpeed
            );
        }
    }

}