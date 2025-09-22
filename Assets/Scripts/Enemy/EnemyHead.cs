using UnityEngine;
public class EnemyHead : MonoBehaviour
{
    [SerializeField] BaseEnemy BaseEnemy;
    public void GetDamage(float Damage, bool IsHeadShot, Vector3 Direction, Vector3 Position)
    {
        BaseEnemy.GetDamage(Damage, true,Position,Direction);
        BaseEnemy.DeathDirection = Direction;
        BaseEnemy.DeathPoint = Position;
    }
}