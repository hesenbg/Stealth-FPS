using UnityEngine;
using System.Collections;
using System;
public class ShootLogic : MonoBehaviour
{
    [SerializeField] CombatData PlayerCombatData;
    [SerializeField] Transform Origin;

    public int CurrentMagazineAmmo { get; private set; }
    public int CurrentTotalAmmo { get; private set; }

    private float shootTimer = 0f;
    public event EventHandler OnReloadEnd;
    public event EventHandler OnReload;

    public Vector3 TotalCurrentRecoil;
    private void Start()
    {
        CurrentMagazineAmmo = PlayerCombatData.Magazine;
        CurrentTotalAmmo = PlayerCombatData.TotalAmmo;
    }

    private void Update()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        TotalCurrentRecoil = Vector3.Lerp(TotalCurrentRecoil,
            Vector3.zero,
            Time.deltaTime*PlayerCombatData.RecoilRecoverySpeed);
    }

    public bool CanShoot()
    {
        if (shootTimer <= 0 && CurrentMagazineAmmo > 0)
        {
            shootTimer = PlayerCombatData.ShootDelay;

            CurrentMagazineAmmo--;
            return true;
        }
        return false;
    }

    public void Shoot()
    {
        Ray ray = new Ray(Origin.position, Origin.forward+ TotalCurrentRecoil);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                PlayerCombatData.Trace.ApplyRandomTexture();
                Instantiate(PlayerCombatData.Trace, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            if (hit.collider.CompareTag("Head"))
            {
                hit.collider.gameObject.GetComponentInParent<HealthManager>().GetHeadShotDamage(PlayerCombatData.BaseDamage,PlayerCombatData.HsMultipiler);
            }
            if (hit.collider.CompareTag("Body"))
            {
                hit.collider.gameObject.GetComponentInParent<HealthManager>().GetDamage(PlayerCombatData.BaseDamage);
            }
            if (hit.collider.CompareTag("Destructable"))
            {
                hit.collider.gameObject.GetComponent<Destructable>().DestroyObject();
            }
        }
    }

    public void CalculateRecoil()
    {
        float randomX = UnityEngine.Random.Range(0,
            PlayerCombatData.RecoilX);
        float randomY = UnityEngine.Random.Range(0,
            PlayerCombatData.RecoilY);
        float randomZ = UnityEngine.Random.Range(0,
            PlayerCombatData.RecoilZ);

        Vector3 Recoil = new Vector3(randomX, randomY, randomZ);

        TotalCurrentRecoil += Recoil*PlayerCombatData.RecoilBuildupSpeed;
    }

    int toLoad;

    public IEnumerator Reload()
    {
        OnReload?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(PlayerCombatData.ReloadTime);
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }

    public void MagOut()
    {
        int needed = PlayerCombatData.Magazine - CurrentMagazineAmmo;
        toLoad = Mathf.Min(needed, CurrentTotalAmmo);
        CurrentMagazineAmmo += toLoad;
    }

    public void MagIn()
    {
        CurrentTotalAmmo -= toLoad;
    }
}