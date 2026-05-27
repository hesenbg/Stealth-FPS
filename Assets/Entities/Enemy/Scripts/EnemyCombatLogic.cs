using UnityEngine;

public class EnemyCombatLogic : MonoBehaviour
{
    [SerializeField] float FireRate = 1f; // time between shots in seconds
    float fireTimer = 0f;
    [SerializeField] LayerMask TargetLayer;

    [SerializeField] int damage;

    public void Shoot(Vector3 Direction)
    {
        if(Physics.Raycast(transform.position, Direction,out RaycastHit hit, 100f, TargetLayer,QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.TryGetComponent<HealthManager>(out HealthManager healthmanager))
            {
                healthmanager.ApplyDamage(damage,1f,hit.point);
            }
        }
    }


    void Update()
    {
        if (fireTimer > 0f)
            fireTimer -= Time.deltaTime;
    }

    public bool CanShoot()
    {
        if (fireTimer > 0f) return false;
        fireTimer = FireRate;
        return true;
    }
}
