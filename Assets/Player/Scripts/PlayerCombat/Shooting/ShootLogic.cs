using UnityEngine;
using System.Collections;
using System;
public class ShootLogic : MonoBehaviour
{
    [SerializeField] CombatData PlayerCombatData;

    [SerializeField] Transform Origin;

    public int CurrentMagazineAmmo {  get; private set; }
    public int CurrentTotalAmmo { get; private set; }

    bool IsShootable = true;
    [HideInInspector] public bool IsShooting = false;
    float shootCooldown = 0f;

    // events
    public event EventHandler OnReloadEnd;
    public event EventHandler OnReload;

    private void Start()
    {
        CurrentMagazineAmmo = PlayerCombatData.Magazine;
        CurrentTotalAmmo = PlayerCombatData.TotalAmmo;
    }

    public void Shoot()
    {
        CurrentMagazineAmmo--;

        Ray ray = new Ray(Origin.transform.position,Origin.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // when hits an objects stop raycasting and apply impact
            if (hit.collider.CompareTag("Obstacle"))
            {
                PlayerCombatData.Trace.ApplyRandomTexture();
                Instantiate(PlayerCombatData.Trace, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
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
            else if (hit.collider.CompareTag("Destrcutable")) // destructibles
            {
                hit.collider.gameObject.GetComponent<Destructable>().DestroyObject();
            }
        }
    }

    public IEnumerator Reload()
    {
        OnReload?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(PlayerCombatData.ReloadTime);

        int needed = PlayerCombatData.Magazine - CurrentMagazineAmmo;
        int toLoad = Mathf.Min(needed, PlayerCombatData.TotalAmmo);

        CurrentMagazineAmmo += toLoad;
        CurrentTotalAmmo-= toLoad;

        OnReloadEnd?.Invoke(this,EventArgs.Empty);
    }
}
