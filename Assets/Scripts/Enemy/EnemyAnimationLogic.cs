using UnityEngine;

public class EnemyAnimationLogic : MonoBehaviour
{
    Animator EnemyAnimator;

    BaseAI BaseAI;

    bool IsRunning;


    private void Start()
    {
        EnemyAnimator = GameObject.Find("EnemyMesh").GetComponent<Animator>();

        BaseAI = gameObject.GetComponent< BaseAI>();
    }

    private void Update()
    {
        EnemyAnimator.SetBool("IsRunning", IsRunning);

        if(BaseAI.CurrentState == BaseAI.GuardState.Wander || BaseAI.CurrentState == BaseAI.GuardState.Suspicious)
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }

        if (BaseAI.AlarmState.IsLeaning)   // lean value 0 left 1 right
        {
            EnemyAnimator.SetBool("IsLeaning",BaseAI.AlarmState.IsLeaning);

            if (BaseAI.AlarmState.IsRightBarricadeSide)
            {
                EnemyAnimator.SetFloat("LeanValue", 1f);
            }
            else
            {
                EnemyAnimator.SetFloat("LeanValue", 0f);
            }
        }

    }
}
