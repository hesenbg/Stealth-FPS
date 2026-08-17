using UnityEngine;
public class EnemyCombatLogic : MonoBehaviour
{
    [SerializeField] float FireRate = 1f; // time between shots in seconds
    float fireTimer = 0f;
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] int damage;
     Vector3 _lastShotDirection = Vector3.zero;
    public void Shoot(Vector3 Pos, EnemyType Type)
    {
        Vector3 Direction = (Pos - transform.position);

        _lastShotDirection = Direction;

        if (Physics.Raycast(transform.position, Direction, out RaycastHit hit, 100f, TargetLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent<HealthManager>(out HealthManager healthmanager))
            {
                healthmanager.ApplyDamage(damage, 1f, hit.point);
            }
        }

        if (Type == EnemyType.Sniper)
            EnemyVisualAudios.instance.PlaySniperFireSound(transform.position);
        else
            EnemyVisualAudios.instance.PlayPistolFireSound(transform.position);
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
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _lastShotDirection * 100f);
    }
}