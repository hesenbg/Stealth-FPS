using UnityEngine;

[CreateAssetMenu(menuName = "PlayerCombat/CombatData")]
public class CombatData : ScriptableObject
{
    [Header("Shooting")]
    [SerializeField] private float shootDelay;
    [SerializeField] private BulletHole trace;

    [Header("Ammo")]
    [SerializeField] private int magazine;
    [SerializeField] private int totalAmmo;
    [SerializeField] private float reloadTime;

    [Header("Recoil")]
    [SerializeField] private float baseRecoil;
    [SerializeField] private float moveRecoilMultiplier;
    [SerializeField] private float recoilRecoverySpeed;
    [SerializeField] private float recoilBuildupSpeed;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;
    // getters

    public float RecoilX => recoilX;
    public float RecoilY => recoilY;
    public float RecoilZ => recoilZ;

    public float RecoilBuildupSpeed => recoilBuildupSpeed;

    public float ShootDelay => shootDelay;
    public BulletHole Trace => trace;

    public int Magazine => magazine;
    public int TotalAmmo => totalAmmo;
    public float ReloadTime => reloadTime;

    public float BaseRecoil => baseRecoil;
    public float MoveRecoilMultiplier => moveRecoilMultiplier;
    public float RecoilRecoverySpeed => recoilRecoverySpeed;
}
