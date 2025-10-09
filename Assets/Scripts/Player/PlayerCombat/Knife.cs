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
    Collider[] AttackedEnemies;

    public void Attack()
    {
        AttackedEnemies = Physics.OverlapBox(transform.position, HitBoxhalfExtend, transform.rotation, AffectObjects);

        foreach (Collider c in AttackedEnemies)
        {
            c.gameObject.GetComponent<EnemyHealthManager>().GetKnifeDamage();
            return;
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(transform.position, HitBoxhalfExtend * 2);
    }



}
