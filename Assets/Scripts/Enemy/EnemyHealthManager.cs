using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;
public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;
    [SerializeField] float HSmultipler;
    Rigidbody rb;
    GuardingEnemy guardingEnemy;
    GameObject BaseMesh;
    Animator animator;

    [SerializeField] GameObject EnemyAI;

    [SerializeField] GameObject Target;

    [SerializeField] GameObject DeadEnemy;
    private void Start()
    {
        BaseMesh = GameObject.Find("Robot_Soldier_Camo1");
        rb = GetComponent<Rigidbody>();
        guardingEnemy = GetComponent<GuardingEnemy>();
        animator = GameObject.Find("EnemyMesh").GetComponent<Animator>();
        //animator = BaseMesh.GetComponent<Animator>();
        CurrentHealth = MaxHealth;
    }
    private void FixedUpdate()
    {
        Die();
    }
    public void GetDamage(float damage,bool IsHeadShot, Vector3 position, Vector3 Direction)
    {
        DeathPoint = position;
        DeathDirection = Direction;

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
        if (CurrentHealth <= 0 && !HasLayDown)
        {
            LayDown(DeathDirection);
            Destroy(EnemyAI);


            // Spawn the dead enemy
            Instantiate(DeadEnemy, transform.position, transform.rotation);

            HasLayDown = true;
        }
    }


    bool HasLayDown;
    [SerializeField] float DeathForce;
    public Vector3 DeathDirection;
    public Vector3 DeathPoint;
    
    public void LayDown(Vector3 direction) // should have destroy the enemy and instantiate dead gameobject
    {

    }
}