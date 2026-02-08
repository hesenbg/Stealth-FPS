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

    public Vector3 TotalCurrentRecoil { get; private set; }

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
    }

    public bool Shoot()
    {
        if (shootTimer <= 0 && CurrentMagazineAmmo > 0)
        {
            shootTimer = PlayerCombatData.ShootDelay;

            ExecuteRaycast();
            CurrentMagazineAmmo--;
            return true;
        }

        return false;
    }

    private void ExecuteRaycast()
    {
        Ray ray = new Ray(Origin.position, Origin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                PlayerCombatData.Trace.ApplyRandomTexture();
                Instantiate(PlayerCombatData.Trace, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            else if (hit.collider.CompareTag("Head"))
            {
                SoundManager.Instance.PlayHeadShotIndicator(transform.position);
                hit.collider.gameObject.GetComponent<EnemyHead>().GetDamage(40, true, hit.normal, hit.point);
            }
            else if (hit.collider.CompareTag("Body"))
            {
                hit.collider.gameObject.GetComponent<EnemyHealthManager>().GetDamage(40, false, hit.point, hit.normal);
            }
            else if (hit.collider.CompareTag("Destructable"))
            {
                hit.collider.gameObject.GetComponent<Destructable>().DestroyObject();
            }
        }
    }


    void CalculateRecoil()
    {

    }

    public IEnumerator Reload()
    {
        OnReload?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(PlayerCombatData.ReloadTime);

        int needed = PlayerCombatData.Magazine - CurrentMagazineAmmo;
        int toLoad = Mathf.Min(needed, CurrentTotalAmmo);

        CurrentMagazineAmmo += toLoad;
        CurrentTotalAmmo -= toLoad;

        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }
}