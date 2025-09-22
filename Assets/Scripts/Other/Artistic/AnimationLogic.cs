using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] ShootLogic ShootLogic;
    [SerializeField] Animator EnemyAnimator; 
    [SerializeField] GameObject EnemyParentObject;
    public bool IsRunning;

    private void Start()
    {

    }

    private void Update()
    {
        PlayerAnimator.SetBool("IsRunning",IsRunning);
    }

    public void PlayReloadAnimation(bool IsMagEmpty)
    {
        if (IsMagEmpty)
        {
            PlayerAnimator.SetFloat("ReloadVal", 0);
        }
        else
        {
            PlayerAnimator.SetFloat("ReloadVal", 1);
        }
        PlayerAnimator.SetTrigger("Reload");
    }

    public void PlayKnifeAttackAnimation()
    {
        PlayerAnimator.SetTrigger("KnifeAttack");
    }

    public void PlayeRecoilAnimation()
    {
         PlayerAnimator.SetTrigger("Fire");
    }

    public void UpdateEnemyAnimations()
    {

    }
}
