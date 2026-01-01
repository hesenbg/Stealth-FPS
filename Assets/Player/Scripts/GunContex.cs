using UnityEngine;

public class GunContext
{
    private float shootDelay;
    private BulletHoleBehaviour bulletTrace;
    private int magazineSize;
    private int totalAmmo;
    private int currentAmmo;
    private float reloadTime;
    private bool isShootable;
    private float shootCooldown;

    public GunContext(
        float shootDelay,
        BulletHoleBehaviour bulletTrace,
        int magazineSize,
        int totalAmmo,
        int currentAmmo,
        float reloadTime,
        bool isShootable,
        float shootCooldown)
    {
        this.shootDelay = shootDelay;
        this.bulletTrace = bulletTrace;
        this.magazineSize = magazineSize;
        this.totalAmmo = totalAmmo;
        this.currentAmmo = currentAmmo;
        this.reloadTime = reloadTime;
        this.isShootable = isShootable;
        this.shootCooldown = shootCooldown;
    }

    public float GetShootDelay() => shootDelay;
    public BulletHoleBehaviour GetBulletTrace() => bulletTrace;
    public int GetMagazineSize() => magazineSize;
    public int GetTotalAmmo() => totalAmmo;
    public int GetCurrentAmmo() => currentAmmo;
    public float GetReloadTime() => reloadTime;
    public bool GetIsShootable() => isShootable;
    public float GetShootCooldown() => shootCooldown;
}
