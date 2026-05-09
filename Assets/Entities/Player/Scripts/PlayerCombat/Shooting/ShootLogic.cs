using UnityEngine;
using System.Collections;
using System;
public class ShootLogic : MonoBehaviour
{
    [SerializeField] CombatData data;
    [SerializeField] Transform Origin;
    [SerializeField] LayerMask mask;

    public int CurrentMagazineAmmo { get; private set; }
    public int CurrentTotalAmmo { get; private set; }

    private float shootTimer = 0f;
    public event EventHandler OnReloadEnd;
    public event EventHandler OnReload;

    public Vector3 TotalCurrentRecoil;

    private float ShootSpeedMultipiler=1f;

    [Header("Heat Settings")]
    private int CurrentHotValue;
    private int MaxHotValue;
    private float lastShotTime;

    private void Start()
    {
        MaxHotValue = data.RecoilPattern.Length;
        CurrentMagazineAmmo = data.Magazine;
        CurrentTotalAmmo = data.TotalAmmo;
    }

    private void Update()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }

        TotalCurrentRecoil = Vector3.MoveTowards(TotalCurrentRecoil,
            Vector3.zero,
            Time.deltaTime * data.RecoilRecoverySpeed);

        HandleHeatDecay();
    }

    private void HandleHeatDecay()
    {
        if (Time.time > lastShotTime + data.HeatDecayDelay && CurrentHotValue > 0)
        {
            float decayStep = data.HeatDecayRate * Time.deltaTime;

            if (decayStep >= 1f)
            {
                CurrentHotValue -= Mathf.FloorToInt(decayStep);
            }
            else if (Time.frameCount % 10 == 0)
            {
                CurrentHotValue--;
            }

            CurrentHotValue = Mathf.Clamp(CurrentHotValue, 0, MaxHotValue - 1);
        }
    }

    public bool CanShoot()
    {
        if (shootTimer <= 0 && CurrentMagazineAmmo > 0)
        {
            shootTimer = data.ShootDelay * (1 / ShootSpeedMultipiler);
            CurrentMagazineAmmo--;
            return true;
        }
        return false;
    }

    public void Shoot()
    {
        lastShotTime = Time.time;
        Ray ray = new Ray(Origin.position, Origin.forward + TotalCurrentRecoil);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Destructable"))
            {
                Destructable destructable = hit.collider.GetComponent<Destructable>();
                if (destructable == null) Debug.LogWarning($"Destructable component missing on {hit.collider.name}", hit.collider.gameObject);
                else destructable.DestroyObject();
            }
            if (hit.collider.CompareTag("Untagged"))
            {
                data.Trace.ApplyRandomTexture();
                Instantiate(data.Trace, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            if (hit.collider.CompareTag("Head"))
            {
                HealthManager hm = hit.collider.GetComponentInParent<HealthManager>();
                if (hm == null) Debug.LogWarning($"HealthManager missing on parent of {hit.collider.name}", hit.collider.gameObject);
                else hm.GetHeadShotDamage(data.BaseDamage, data.HsMultipiler);
            }
            if (hit.collider.CompareTag("Body"))
            {
                HealthManager hm = hit.collider.GetComponentInParent<HealthManager>();
                hm.GetDamage(data.BaseDamage);

            }
        }
        
        EnemyManager.instance.AlertClosestOnGunFire(transform.position, transform.forward);

        if (CurrentHotValue < MaxHotValue - 1)
        {
            CurrentHotValue++;
        }
    }

    float RecoilDamper = 1;

    public void CalculateRecoilDaper(bool IsADS, float VelocityMagnitude)
    {
        if (IsADS)
        {
            RecoilDamper = data.ADSrecoilDamper;
            ShootSpeedMultipiler = data.ADSshootSpeed;
        }
        else
        {
            RecoilDamper = 1f;
            ShootSpeedMultipiler = 1f;
        }
    }

    public void CalculateRecoil()
    {
        int index = Mathf.Clamp(CurrentHotValue, 0, MaxHotValue - 1);

        float patternX = data.RecoilPattern[index].x;
        float patternY = data.RecoilPattern[index].y;

        float curvedPercentage =data.RecoilPatternRandomness.Evaluate(CurrentHotValue);

        float minX = -patternX*curvedPercentage;
        float maxX = patternX;

        float minY = patternY*curvedPercentage;
        float maxY = patternY;

        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = UnityEngine.Random.Range(minY, maxY);

        Vector3 Recoil = new Vector3(randomX, randomY, 0f);
        
        TotalCurrentRecoil += Recoil * data.RecoilBuildupSpeed*RecoilDamper;
    }

    int toLoad;

    public IEnumerator Reload()
    {
        OnReload?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(data.ReloadTime);
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }

    public void MagOut()
    {
        int needed = data.Magazine - CurrentMagazineAmmo;
        toLoad = Mathf.Min(needed, CurrentTotalAmmo);
        CurrentMagazineAmmo += toLoad;
    }

    public void MagIn()
    {
        CurrentTotalAmmo -= toLoad;
    }
}