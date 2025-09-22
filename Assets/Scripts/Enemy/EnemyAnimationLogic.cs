using UnityEngine;

public class EnemyAnimationLogic : MonoBehaviour
{
    Animator EnemyAnimator;

    GuardingEnemy BaseAI;

    bool IsRunning;


    private void Start()
    {
        EnemyAnimator = GameObject.Find("EnemyMesh").GetComponent<Animator>();

        BaseAI = gameObject.GetComponent<GuardingEnemy>();
    }

    private void Update()
    {
        EnemyAnimator.SetBool("IsRunning", IsRunning);

        if(BaseAI.currentState == GuardingEnemy.GuardState.Wander || BaseAI.currentState == GuardingEnemy.GuardState.Triggered)
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }
    }

}
