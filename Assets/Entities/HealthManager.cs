using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    void CheckDie()
    {
        if(CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void GotDamage(float damage)
    {
        CurrentHealth -= damage;
        CheckDie();
    }
}
