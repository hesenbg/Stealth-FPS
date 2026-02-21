using UnityEngine;
public class HealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;

    [SerializeField] Transform Head;
    [SerializeField] Transform Body;

    [SerializeField] GameObject OriginalHips;
    [SerializeField] GameObject CoreObjectToBeDestroyed;
    [SerializeField] GameObject Ragdoll;

    private void Start()
    {
        CurrentHealth = MaxHealth;
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

    public void GetGrenadeDamage(float DropOut) // how far away nade exploded from our enemy
    {
        DropOut = 1 - DropOut;
        CurrentHealth -= MaxHealth*DropOut;
        CheckDie();
    }

    public void GetKnifeDamage()
    {
        CurrentHealth-= MaxHealth;
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
        GameObject spawnedRagdoll = Instantiate(Ragdoll, transform.position, transform.rotation);

        EnemyRagdoll ragdollScript = spawnedRagdoll.GetComponent<EnemyRagdoll>();



        if (ragdollScript != null)
        {
            ragdollScript.MatchRagdollToAnimation(OriginalHips);
        }
    }
}