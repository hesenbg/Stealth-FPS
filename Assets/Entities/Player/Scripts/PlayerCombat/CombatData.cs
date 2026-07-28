using UnityEngine;

[CreateAssetMenu(menuName = "PlayerCombat/CombatData")]
public class CombatData : ScriptableObject
{
    [Header("Shooting")]
    [SerializeField] private float shootDelay;
    [SerializeField] private GameObject trace;
    [SerializeField] private float adsShootSpeed;

    [Header("Ammo")]
    [SerializeField] private int magazine;
    [SerializeField] private int totalAmmo;
    [SerializeField] private float reloadTime;

    [Header("Damage")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float hsMultipiler;

    [Header("Recoil")]
    [SerializeField] private float recoilRecoverySpeed;
    [SerializeField] private float recoilBuildupSpeed;
    [SerializeField] float heatDecayDelay;
    [SerializeField] float heatDecayRate;
    [SerializeField] Vector2[] recoilPattern;
    [SerializeField] AnimationCurve recoilPatternNegativeRandomness;
    [SerializeField] float adsRecoilDamper;

    // getters
    public float ADSshootSpeed => adsShootSpeed;
    public float ADSrecoilDamper => adsRecoilDamper;
    public AnimationCurve RecoilPatternRandomness => recoilPatternNegativeRandomness;
    public float HeatDecayDelay => heatDecayDelay;
    public float HeatDecayRate => heatDecayRate;
    public Vector2[] RecoilPattern => recoilPattern;
    public float BaseDamage => baseDamage;
    public float HsMultipiler => hsMultipiler;

    public float RecoilBuildupSpeed => recoilBuildupSpeed;

    public float ShootDelay => shootDelay;
    public GameObject Trace => trace;

    public int Magazine => magazine;
    public int TotalAmmo => totalAmmo;
    public float ReloadTime => reloadTime;
    public float RecoilRecoverySpeed => recoilRecoverySpeed;
}
