using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] ShootLogic ShootLogic;
    [SerializeField] PlayerMovement PlayerMovement;
    public bool IsRunning;

    private void Start()
    {

    }

    private void Update()
    {
        IsRunning = PlayerMovement.IsRunning;

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerAnimator.SetTrigger("MoveLeft");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerAnimator.SetTrigger("MoveRight");
        }
        PlayerAnimator.SetBool("IsRunning",PlayerMovement.IsMoving);
        PlayerAnimator.SetFloat("MoveDirection",PlayerMovement.Velocity.x);
        PlayerAnimator.SetBool("IsReloading",ShootLogic.isReloading);
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
