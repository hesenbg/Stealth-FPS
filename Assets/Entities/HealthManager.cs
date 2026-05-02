using UnityEngine;
public class HealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;

    [SerializeField] Transform Head;
    [SerializeField] Transform Body;

    [SerializeField] GameObject OriginalHips;
    [SerializeField] GameObject CoreObjectToBeDestroyed;
    [SerializeField] GameObject RagdollObject;
    EnemyRagdoll ragdoll;

    bool HasKnifed = false;

    [SerializeField] Rigidbody rb;

    private void Start()
    {
        ragdoll = RagdollObject.GetComponent<EnemyRagdoll>();
        CurrentHealth = MaxHealth;
        rb = GetComponentInParent<Rigidbody>();
    }

    public void GetDamage(float damage)
    {
        CurrentHealth -= damage;
        EnemyEffects.instance.PlayBodyHit(transform.position);
        CheckDie();
    }

    public void GetHeadShotDamage(float damage, float HeadShotMultipiler)
    {
        CurrentHealth -= damage*HeadShotMultipiler;
        EnemyEffects.instance.PlayHeadHit(transform.position);
        EnemyEffects.instance.PlayBloodVFX(Head.position);
        CheckDie();
    }

    public void GetGrenadeDamage(float Damage) // how far away nade exploded from our enemy
    {
        CurrentHealth -= Damage;
        CheckDie();
    }

    public void GetKnifeDamage()
    {
        CurrentHealth-= MaxHealth;
        HasKnifed = true;
        ragdoll.hasKnifed = HasKnifed;
        CheckDie();
    }

    private void CheckDie()
    {
        if (CurrentHealth <= 0)
        {
            SpawnRagdoll();
            Destroy(CoreObjectToBeDestroyed);
        }
    }

    private void SpawnRagdoll()
    {
        rb.isKinematic = false;
        GameObject spawnedRagdoll = Instantiate(RagdollObject, transform.position, transform.rotation);

        EnemyRagdoll ragdollScript = spawnedRagdoll.GetComponent<EnemyRagdoll>();
        ragdollScript.MatchRagdollToAnimation(OriginalHips);
    }
}