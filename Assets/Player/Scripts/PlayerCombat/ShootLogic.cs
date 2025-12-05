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
    Vector3 ShootDirection;

    [Header("Ammo Settings")]
    [SerializeField] int MagazineSize = 30;
    [SerializeField] int TotalAmmo = 90;
    [SerializeField] float ReloadTime = 2f;
    [SerializeField] TextMeshProUGUI BulletCount;
    [SerializeField] int currentAmmo;

    [Header("References")]


    [Header("Recoil")]
    [SerializeField] Vector3 TotalRecoil;
    [SerializeField] float BaseRecoil = 0.02f;   // base recoil offset per shot
    [SerializeField] float MoveRecoilFactor = 0.02f;
    [SerializeField] float ShootRecoilFactor = 0.05f;
    [SerializeField] float RecoilRecoverySpeed = 2f; // how fast recoil fades back


    // -------------------- Private State --------------------
    [HideInInspector] public bool isReloading = false;
    bool IsShootable = true;
    [HideInInspector] public bool IsShooting = false;
    float shootCooldown = 0f;

    // -------------------- Unity Methods --------------------
    private void Start()
    {
        currentAmmo = MagazineSize;
        PlayerData.SetShootLogic(this);
    }

    private void Update()
    {
        UpdateShootDirection();

        TakeReloadInput();
        ShootInput();
        UpdateShootUI();

        ResetRecoil();
    }

    [SerializeField] float MinFloat;

    void ResetRecoil()
    {
        if (!IsShooting && TotalRecoil.magnitude>MinFloat)
            TotalRecoil = Vector3.Lerp(TotalRecoil, Vector3.zero, Time.deltaTime * RecoilRecoverySpeed);
    }


    private void OnDrawGizmos()
    {
        if (Origin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Origin.transform.position, ShootDirection * RayLength);
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
            PlayerData.GetAnimationLogic().PlayReloadAnimation(currentAmmo == 0);
        }
    }

    void ShootInput()
    {
        if (Input.GetMouseButtonDown(0) && IsShootable && currentAmmo > 0 && !isReloading)
        {
            // play sound
            SoundManager.Instance.PlayGunShot(transform.position);
            // play animation 
            // ShootType float 0 shoot 0.5 shoodry 1 shootlast
            PlayerData.GetAnimationLogic().PlayShootAnimation(currentAmmo);
            // aplly logic
            Shoot();

            // recoil
            //CameraPowLogic.ApllyRecoilMoation(CalculateRecoil());

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

        Ray ray = new Ray(Origin.transform.position, ShootDirection);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RayLength))
        {
            // when hits an objects stop raycasting and apply impact
            if (hit.collider.CompareTag("Obstacle"))
            {
                BulletImpact.ApplyRandomTexture();
                Instantiate(BulletImpact, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                return;
            }
            // damage certain points of enemy
            if (hit.collider.CompareTag("Head"))
            {
                SoundManager.Instance.PlayHeadShotIndicator(transform.position);
                hit.collider.gameObject.GetComponent<EnemyHead>().GetDamage(40, true, hit.normal, hit.point);
            }
            else if (hit.collider.CompareTag("Body"))
            {
                hit.collider.gameObject.GetComponent<EnemyHealthManager>().GetDamage(40, false, hit.point, hit.normal);
            }
            // specificly for destructable objects 
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

    float CalculateRecoil() // todo fix it
    {
        Vector3 recoil = Vector3.zero;

        // More recoil if moving
        //if (Movement.IsMoving)
            recoil += new Vector3(
                Random.Range(-MoveRecoilFactor, MoveRecoilFactor),
                Random.Range(-MoveRecoilFactor, MoveRecoilFactor),
                0f
            );

        // Base recoil per shot
        recoil += new Vector3(
            Random.Range(-BaseRecoil, BaseRecoil),
            Random.Range(BaseRecoil * 0.5f, BaseRecoil), // mostly upward bias
            0f
        );

        // Extra if you’re holding fire (rapid shots)
        if (IsShooting)
            recoil += new Vector3(
                Random.Range(-ShootRecoilFactor, ShootRecoilFactor),
                ShootRecoilFactor,
                0f
            );

        // accumulate
        TotalRecoil += recoil;

        return recoil.y * 10f; // return vertical kick for camera (CameraPowLogic)
    }

    // shoot direction is the sum of camera's forward and recoil
    void UpdateShootDirection()
    {
        ShootDirection = Origin.transform.forward + TotalRecoil;      
    }

    // -------------------- UI --------------------
    void UpdateShootUI()
    {
        //BulletCount.text = $"{currentAmmo}/{TotalAmmo}";
    }
}
