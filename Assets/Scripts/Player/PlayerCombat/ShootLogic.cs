using TMPro;
using UnityEngine;
using System.Collections;

public class ShootLogic : MonoBehaviour
{
    // -------------------- Serialized Fields --------------------
    [Header("Shooting Settings")]
    [SerializeField] float ShootingDelay;
    [SerializeField] float RayLength = 100f;
    [SerializeField] LayerMask HitMask;
    [SerializeField] GameObject Origin;
    [SerializeField] BulletHoleBehaviour BulletImpact;

    [Header("Ammo Settings")]
    [SerializeField] int MagazineSize = 30;
    [SerializeField] int TotalAmmo = 90;
    [SerializeField] float ReloadTime = 2f;
    [SerializeField] TextMeshProUGUI BulletCount;
    [SerializeField] int currentAmmo;

    [Header("References")]
    [SerializeField] AnimationLogic AnimationLogic;

    // -------------------- Private State --------------------
    bool isReloading = false;
    bool IsShootable = true;
    bool IsShooting = false;
    float shootCooldown = 0f;

    // -------------------- Unity Methods --------------------
    private void Start()
    {
        currentAmmo = MagazineSize;
    }

    private void Update()
    {
        TakeReloadInput();
        ShootInput();
        UpdateShootUI();
    }

    private void OnDrawGizmos()
    {
        if (Origin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Origin.transform.position, Origin.transform.forward * RayLength);
        }
    }

    // -------------------- Input Handling --------------------
    void TakeReloadInput()
    {
        if (isReloading) return;

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
            AnimationLogic.PlayReloadAnimation(currentAmmo == 0);
        }
    }

    void ShootInput()
    {
        if (Input.GetMouseButtonDown(0) && IsShootable && currentAmmo > 0 && !isReloading)
        {
            // play sound
            SoundManager.Instance.PlayGunShot(transform.position);

            AnimationLogic.PlayeRecoilAnimation();
            Shoot();
            shootCooldown = ShootingDelay;
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }
    }

    // -------------------- Core Logic --------------------
    private void Shoot()
    {
        currentAmmo--;

        Ray ray = new Ray(Origin.transform.position, Origin.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RayLength))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                BulletImpact.ApplyRandomTexture();
                Instantiate(BulletImpact, hit.point+(hit.normal*0.01f), Quaternion.FromToRotation(Vector3.up,hit.normal));
                return;
            }

            if (hit.collider.CompareTag("Head"))
            {
                SoundManager.Instance.PlayHeadShotIndicator(transform.position);
                hit.collider.gameObject.GetComponent<EnemyHead>().GetDamage(40, true, hit.normal, hit.point);
            }
            else if (hit.collider.CompareTag("Body"))
            {
                hit.collider.gameObject.GetComponent<EnemyHealthManager>().GetDamage(40, false, hit.point, hit.normal);
            }
            else if (hit.collider.gameObject.layer == 9) // destructibles
            {
                hit.collider.gameObject.GetComponent<Destructable>().DestroyObject();
            }
        }
    }

    private IEnumerator Reload()
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

    // -------------------- UI --------------------
    void UpdateShootUI()
    {
        BulletCount.text = $"{currentAmmo}/{TotalAmmo}";
    }
}
