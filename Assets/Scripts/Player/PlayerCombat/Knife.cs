using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] float AttackDistance;
    [SerializeField] LayerMask AffectObjects;
    [SerializeField] Vector3 HitBoxhalfExtend;

    [SerializeField] float AttackDelay;
    float AttackDelayValue = 0;

    [SerializeField] KeyCode AttackKey;

    [SerializeField] AnimationLogic Logic;

    public bool Attack()
    {
        if (Physics.CheckBox(transform.position, HitBoxhalfExtend, transform.rotation, AffectObjects, QueryTriggerInteraction.Ignore))
        {
            return true;    
        }
        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(AttackKey) && AttackDelayValue>=AttackDelay)
        {
            Logic.PlayKnifeAttackAnimation();

            Attack();

            AttackDelayValue = 0;
        }
        else
        {
            AttackDelayValue += Time.deltaTime;
        }
    }



}
