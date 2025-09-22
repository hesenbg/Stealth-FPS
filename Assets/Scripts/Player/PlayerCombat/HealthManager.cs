using UnityEngine;

public class HealthManager : MonoBehaviour
{
    GameObject Player;
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
            Destroy(Player);
        }
    }
    private void Update()
    {
        CheckDie();
    }

    public void GotDamage(float damage)
    {
        CurrentHealth -= damage;
    }
}
