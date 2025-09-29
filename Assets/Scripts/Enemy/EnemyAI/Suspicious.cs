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

    [SerializeField] float SuspiciousSpeed;
    public void UpdateSuspicious() // checks position and goes back 
    {
        Base.UpdateRotation(Base.TriggerPosition,RotationSpeed);
        if (Base.Tracktarget(Base.TriggerPosition,SuspiciousSpeed))
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


}