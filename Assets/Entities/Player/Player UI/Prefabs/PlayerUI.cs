using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] TextMeshProUGUI PlayerGunText;

    [Header("Health")]
    [SerializeField] HealthManager HealthManager;
    [SerializeField] Slider HealthBar;
    [SerializeField] Image PlayerDamage;

    [Header("Observability")]
    [SerializeField] ObservableObject ObservableObject;
    [SerializeField] Slider ObservabilityBar;

    [Header("Grenade")]
    [SerializeField] ThrowAbleLogic ThrowAbleLogic;
    [SerializeField] RectTransform CurrentNadeIconRec;
    [SerializeField] float DistanceBetweenNadeIcons;
    [SerializeField] int CurrentNadeIndex;
    [SerializeField] TextMeshProUGUI FlashNadeText;
    [SerializeField] TextMeshProUGUI SmokeNadeText;
    [SerializeField] TextMeshProUGUI DistractionText;

    [Header("Effects")]
    [SerializeField] Image FlashEffect;

    [Header("Crosshair")]
    [SerializeField] GameObject Cross;

    [Header("Indicators")]
    [SerializeField] Canvas PlayerUIcanvas;
    [SerializeField] GameObject indicatorObject;


    private void Start()
    {
        PlayerDamage.color = new Color(1f, 1f, 1f, 0f);
    }

    void UpdateHealthAndObservability()
    {
        HealthBar.value = Mathf.Lerp(HealthBar.value, (float)HealthManager.CurrentHealth / HealthManager.MaxHealth, Time.deltaTime * 5f);
        ObservabilityBar.value = Mathf.Lerp(ObservabilityBar.value, ObservableObject.Observability, Time.deltaTime * 5f);
    }

    private void Update()
    {
        UpdateCross();
        UpdateHealthAndObservability();

        UpdateNadeUI();
    }

    void UpdateNadeUI()
    {
        int index = ThrowAbleLogic.GetIndex();

        float targetY = (index - 1) * DistanceBetweenNadeIcons;
        Vector2 pos = CurrentNadeIconRec.anchoredPosition;
        CurrentNadeIconRec.anchoredPosition = new Vector2(pos.x, Mathf.Lerp(pos.y, targetY, Time.deltaTime * 10f));
        FlashNadeText.text = ThrowAbleLogic.GetCount(1).ToString();
        SmokeNadeText.text = ThrowAbleLogic.GetCount(2).ToString();
        DistractionText.text = ThrowAbleLogic.GetCount(3).ToString();

        FlashNadeText.gameObject.SetActive(index == 1);
        SmokeNadeText.gameObject.SetActive(index == 2);
        DistractionText.gameObject.SetActive(index == 3);
    }

    public void SetPlayerDamageUI(float Ratio)
    {
        PlayerDamage.color = new Color(1f, 1f, 1f, Ratio);
    }

    void UpdateCross()
    {
        Cross.SetActive(PlayerComponents.Instance.InputManager.CurrentGunState != InputManager.GunState.ADS);
    }

    public Indicator CreateIndicator()
    {
        GameObject indicator = Instantiate(indicatorObject, PlayerUIcanvas.gameObject.transform);
        return indicator.GetComponentInChildren<Indicator>();
    }

    public void FlashEffectUI(float Duration)
    {
        StartCoroutine(FlashRoutine(Duration));
    }

    IEnumerator FlashRoutine(float Duration)
    {
        float elapsed = 0f;
        FlashEffect.color = new Color(1f, 1f, 1f, 1f);

        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            FlashEffect.color = new Color(1f, 1f, 1f, 1f - (elapsed / Duration));
            yield return null;
        }

        FlashEffect.color = new Color(1f, 1f, 1f, 0f);
    }

    public void UpdateGunUI(int CurrentMag, int TotalMag)
    {
        PlayerGunText.text = $"{CurrentMag} / {TotalMag}";
    }
}