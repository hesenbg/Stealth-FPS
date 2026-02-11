using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;
public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;
    [SerializeField] float HSmultipler;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    private void FixedUpdate()
    {
        Die();
    }
    public void GetDamage(float damage,bool IsHeadShot, Vector3 position, Vector3 Direction)
    {

        if (IsHeadShot)
        {
            CurrentHealth -= damage*HSmultipler;
        }
        else if (!IsHeadShot)
        {
            CurrentHealth -= damage;
        }
    }

    public void GetGrenadeDamage(float DropOut) // how far away nade exploded from our enemy
    {
        DropOut = 1 - DropOut;
        CurrentHealth -= MaxHealth*DropOut;
    }

    public void GetHeadShot()
    {
        CurrentHealth -= MaxHealth;
    }
    public void GetKnifeDamage()
    {
        CurrentHealth-= MaxHealth;
    }
    void Die()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}