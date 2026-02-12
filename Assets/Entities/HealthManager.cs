using UnityEngine;
public class HealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void GetDamage(float damage, float HSmultipiler)
    {
        CurrentHealth -= damage * HSmultipiler;
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
            Destroy(gameObject);
        }
    }
}