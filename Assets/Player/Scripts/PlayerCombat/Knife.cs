using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] float AttackDistance = 2f;
    [SerializeField] LayerMask AffectObjects;
    [SerializeField] float AttackDelay = 0.5f;
    [SerializeField] KeyCode AttackKey = KeyCode.Mouse0;

    [Header("References")]
    [SerializeField] AnimationLogic Logic;

    float AttackDelayTimer;

    private void Update()
    {
        AttackDelayTimer += Time.deltaTime;

        if (Input.GetKeyDown(AttackKey) && AttackDelayTimer >= AttackDelay)
        {
            Logic.PlayKnifeAttackAnimation();
            DetectAndApplyDamage();
            AttackDelayTimer = 0;
        }
    }

    public void DetectAndApplyDamage()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, AttackDistance, AffectObjects))
        {
            EnemyHealthManager enemy = hit.collider.GetComponent<EnemyHealthManager>();
            if (enemy != null)
            {
                enemy.GetKnifeDamage();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position,transform.position+ transform.forward.normalized*AttackDistance);

    }
}
