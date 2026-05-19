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
    private void Start()
    {
        if (RagdollObject != null)
            ragdoll = RagdollObject.GetComponent<EnemyRagdoll>();
        CurrentHealth = MaxHealth;
    }
    public void GetDamage(float damage, Vector3 direction)
    {
        CurrentHealth -= damage;
        EnemyEffects.instance.PlayBodyHit(transform.position);
        CheckDie();
    }
    public void GetHeadShotDamage(float damage, float HeadShotMultipiler)
    {
        CurrentHealth -= damage * HeadShotMultipiler;
        EnemyEffects.instance.PlayHeadHit(transform.position);
        EnemyEffects.instance.PlayBloodVFX(Head.position);
        CheckDie();
    }
    public void GetKnifeDamage()
    {
        CurrentHealth -= MaxHealth;
        HasKnifed = true;
        if (ragdoll != null)
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
        if (RagdollObject == null || OriginalHips == null) return;
        GameObject spawnedRagdoll = Instantiate(RagdollObject, transform.position, transform.rotation);
        EnemyRagdoll ragdollScript = spawnedRagdoll.GetComponent<EnemyRagdoll>();
        if (ragdollScript != null)
            ragdollScript.MatchRagdollToAnimation(OriginalHips);
    }
}