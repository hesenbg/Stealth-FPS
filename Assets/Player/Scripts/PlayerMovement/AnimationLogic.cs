using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public Animator PlayerAnimator;
    [SerializeField] ShootLogic ShootLogic;
    [SerializeField] PlayerMovement PlayerMovement;
    public bool IsRunning;

    private void Start()
    {
        PlayerData.SetAnimationLogic(this);
    }

    private void Update()
    {

    }   

    public void PlayReloadAnimation(bool IsMagEmpty)
    {
        if (IsMagEmpty)
        {
            PlayerAnimator.SetFloat("ReloadValue", 0);
        }
        else
        {
            PlayerAnimator.SetFloat("ReloadValue", 1);
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
