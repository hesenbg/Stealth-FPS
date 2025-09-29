using UnityEngine;
public class EnemyHead : MonoBehaviour
{
    [SerializeField] EnemyHealthManager HealthManager;
    public void GetDamage(float Damage, bool IsHeadShot, Vector3 Direction, Vector3 Position)
    {
        HealthManager.GetDamage(Damage, true,Position,Direction);
        HealthManager.DeathDirection = Direction;
        HealthManager.DeathPoint = Position;
    }
}