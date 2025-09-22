using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Grenade components and properties
    Rigidbody rb;
    [HideInInspector] public bool CanDamage = false;
    [SerializeField] LayerMask EnemyLayer; // Renamed to clarify
    [SerializeField] LayerMask PlayerLayer; // New field for the player's layer
    public float Force ;

    // nade
    [Header("Smoke")]
    [SerializeField] float SmokeDuration ; // How long the player is "smoked" if in radius
    [SerializeField] float SmokeRadius;
    [SerializeField] float SmokeTrigger;

    [Header("Explosive")]
    [SerializeField] float ExplosionRadius;
    [SerializeField] public float FuseDelay ;


    [Header("Sound && Effect")]
    // Sounds
    [SerializeField] AudioClip ProjectileSound;

    // Effects and references
    Collider[] HitEnemies;
    [SerializeField] GameObject ProjectileEffect;
    PlayerVisibility PS;

    // enums
    public enum GreandeType {smoke, explosive }
    public GreandeType OurGrenade;

    public Vector3 ThrowDirection;
    private void Update()
    {
        Physics.IgnoreLayerCollision(3, 11,true); // 11 is the projectile layer
    }

    private void Awake()
    {
        PS = GameObject.Find("PlayerVisibility").GetComponent<PlayerVisibility>();
        rb = GetComponent<Rigidbody>();
        Release();
        if(OurGrenade == GreandeType.smoke)
        {
            StartCoroutine(Smoke());
        }
        if(OurGrenade == GreandeType.explosive)
        {
            StartCoroutine(Explosion());
        }
    }
    
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(FuseDelay);
        PlayEffectSound(ProjectileEffect, ProjectileSound);

        // deal damage
        DealDamage();

        // create effect && sound
        Destroy(gameObject);
    }

    IEnumerator Smoke()
    {
        yield return new WaitForSeconds(SmokeTrigger);
        // check if player in smoke 
        PS.IsSmoked = CheckPlayerVisibility();

        PlayEffectSound(ProjectileEffect, ProjectileSound);

        rb.constraints = RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(SmokeDuration);
        // create effect sound 
        PS.IsSmoked = false;
        Destroy(gameObject);
    }

    void PlayEffectSound(GameObject NadeEffect, AudioClip NadeSound)
    {
        // force effect to face upward
        Quaternion upRotation = Quaternion.identity; // world up
        Instantiate(NadeEffect, transform.position,Quaternion.LookRotation(Vector3.up));

        AudioSource.PlayClipAtPoint(NadeSound, transform.position);
    }


    void DealDamage()
    {
        HitEnemies = Physics.OverlapSphere(transform.position, ExplosionRadius, EnemyLayer, QueryTriggerInteraction.Ignore);

        foreach (Collider hit in HitEnemies)
        {
            hit.gameObject.GetComponent<BaseEnemy>().GetGrenadeDamage(CalculateDamageDropout(transform.position,hit.transform.position));
        }  
    }
    private bool CheckPlayerVisibility()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, SmokeRadius, PlayerLayer);

        if (hitPlayers.Length > 0)
        {
            return true;
        }
        return false;
    }

    float CalculateDamageDropout(Vector3 nade, Vector3 Target)
    {
        float DistanceTargetNade = Vector3.Distance(nade, Target);

        return DistanceTargetNade/ExplosionRadius;
    }

    void Release()
    {
        rb.AddForce(ThrowDirection * Force, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SmokeRadius);
    }
}
