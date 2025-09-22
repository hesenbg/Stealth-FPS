using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
public class ShootLogic : MonoBehaviour
{
    [SerializeField] float ShootingDelay;
    [SerializeField] GameObject Origin;
    [SerializeField] float RayLength = 100f;
    [SerializeField] LayerMask HitMask;
    [SerializeField] AnimationLogic AnimationLogic;

    [Header("Ammo Settings")]
    [SerializeField] int MagazineSize = 30;   
    [SerializeField] int TotalAmmo = 90;     
    [SerializeField] float ReloadTime = 2f;
    [SerializeField] TextMeshProUGUI BulletCount;
    [SerializeField] int currentAmmo;     
    public bool isReloading = false;

    public bool IsShooting = false;
    public bool IsShootable = true;
    float shootCooldown = 0f;
    [Header("sound && SFX")]
    [SerializeField] AudioSource GunSFX;
    [SerializeField] AudioSource Environment;
    [SerializeField] AudioClip ShootSfX;
    [SerializeField] AudioClip HeadshotSFX;
    [SerializeField] AudioClip ReloadSound;

    [Header("Objects")]
    [SerializeField] GameObject AttackKnife;

    private void Start()
    {
        currentAmmo = MagazineSize; 
    }

    void TakeReloadInput()
    {
        if (isReloading) return; // block shooting while reloading

        // cooldown
        if (shootCooldown > 0f)
        {
            shootCooldown -= Time.deltaTime;
            IsShootable = false;
        }
        else
        {
            IsShootable = true;
        }

        // reload input
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < MagazineSize && TotalAmmo > 0)
        {
            StartCoroutine(Reload());
            AnimationLogic.PlayReloadAnimation(currentAmmo==0);
            return;
        }
    }

    void ShootInput()
    {
        // shooting input
        if (Input.GetMouseButtonDown(0) && IsShootable && currentAmmo > 0 && !isReloading)
        {
            // play sound
            SoundManager.Instance.PlayGunShot(transform.position);

            AnimationLogic.PlayeRecoilAnimation();
            Shoot();
            shootCooldown = ShootingDelay;
            IsShooting = true;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            KnifeAttack();
        }
        else
        {
            IsShooting = false;
        }
    }
    void UpdateShootUI()
    {
        BulletCount.text = $"{currentAmmo}/{TotalAmmo}";

    }
    private void Update()
    {
            TakeReloadInput();
            ShootInput();
            UpdateShootUI();
    }

    [SerializeField] GameObject BulletImpact;

    private void Shoot()
    {
        currentAmmo--;

        Ray ray = new Ray(Origin.transform.position, Origin.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RayLength))
        {
            // walls
            if (hit.collider.CompareTag("Obstacle"))
            {
                //GameObject impact = Instantiate(BulletImpact,hit.point + (hit.normal * 0.01f),Quaternion.LookRotation(hit.normal),hit.transform);
                return;
            }

            if (hit.collider.CompareTag("Head"))
            {
                SoundManager.Instance.PlayHeadShotIndicator(transform.position);
                hit.collider.gameObject.GetComponent<EnemyHead>().GetDamage(40, true,hit.normal,hit.point);
            }
            if (hit.collider.CompareTag("Body"))
            {
                hit.collider.gameObject.GetComponent<BaseEnemy>().GetDamage(40, false,hit.point,hit.normal);
            }
            // destructable objects
            if(hit.collider.gameObject.layer == 9)
            {
                hit.collider.gameObject.GetComponent<Destructable>().DestroyObject();
            }
        }
    }


    [SerializeField] float KinfeDamageDistance;
    [SerializeField] Vector3 KnifeHitBoxHalfExtend;
    Collider[] KnifedEnemies;

    void KnifeAttack()
    {
        // play animation
        AnimationLogic.PlayKnifeAttackAnimation();


        // play sfx


        // deal damage
        KnifedEnemies= Physics.OverlapBox(AttackKnife.transform.position,KnifeHitBoxHalfExtend,AttackKnife.transform.rotation,HitMask,QueryTriggerInteraction.Ignore);

        foreach(Collider c in KnifedEnemies)
        {
            BaseEnemy TargetEnemy;
            TargetEnemy= c.GetComponent<BaseEnemy>();
            if(TargetEnemy != null)
            {
                TargetEnemy.GetKnifeDamage();
                return;
            }
        }
    }


    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        SoundManager.Instance.PlayReload(transform.position);
        yield return new WaitForSeconds(ReloadTime);

        int needed = MagazineSize - currentAmmo;
        int toLoad = Mathf.Min(needed, TotalAmmo);

        currentAmmo += toLoad;
        TotalAmmo -= toLoad;

        isReloading = false;
    }

    private void OnDrawGizmos()
    {
        // Draw gun ray
        if (Origin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Origin.transform.position, Origin.transform.forward * RayLength);
        }

        // Draw knife overlap box
        if (AttackKnife != null)
        {
            Gizmos.color = Color.yellow;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                AttackKnife.transform.position,
                AttackKnife.transform.rotation,
                Vector3.one
            );
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, KnifeHitBoxHalfExtend * 2);
        }
    }
}