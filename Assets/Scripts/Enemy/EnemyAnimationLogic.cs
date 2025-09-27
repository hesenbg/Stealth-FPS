using UnityEngine;

public class EnemyAnimationLogic : MonoBehaviour
{
    Animator EnemyAnimator;

    BaseAI BaseAI;

    bool IsRunning;


    private void Start()
    {
        EnemyAnimator = GetComponent<Animator>();

        BaseAI = GameObject.Find("EnemyAI").gameObject.GetComponent<BaseAI>();
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
    }

    public void LeanRight()
    {
        EnemyAnimator.SetTrigger("LeanRight");
    }

    public void LeanLeft()
    {
        EnemyAnimator.SetTrigger("LeanLeft");
    }

}
