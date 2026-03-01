using UnityEngine;
using System.Collections;
using System;

public class ShootLogic : MonoBehaviour
{
    [SerializeField] CombatData data;
    [SerializeField] Transform Origin;

    public int CurrentMagazineAmmo { get; private set; }
    public int CurrentTotalAmmo { get; private set; }

    private float shootTimer = 0f;
    public event EventHandler OnReloadEnd;
    public event EventHandler OnReload;

    public Vector3 TotalCurrentRecoil;

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
            shootTimer = data.ShootDelay;
            CurrentMagazineAmmo--;
            return true;
        }
        return false;
    }

    public void Shoot()
    {
        lastShotTime = Time.time;

        Ray ray = new Ray(Origin.position, Origin.forward + TotalCurrentRecoil);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                data.Trace.ApplyRandomTexture();
                Instantiate(data.Trace, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            if (hit.collider.CompareTag("Head"))
            {
                hit.collider.gameObject.GetComponentInParent<HealthManager>().GetHeadShotDamage(data.BaseDamage, data.HsMultipiler);
            }
            if (hit.collider.CompareTag("Body"))
            {
                hit.collider.gameObject.GetComponentInParent<HealthManager>().GetDamage(data.BaseDamage);
            }
            if (hit.collider.CompareTag("Destructable"))
            {
                hit.collider.gameObject.GetComponent<Destructable>().DestroyObject();
            }
        }

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
        }
        else
        {
            RecoilDamper = 1f;
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

        Debug.Log(Recoil);
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