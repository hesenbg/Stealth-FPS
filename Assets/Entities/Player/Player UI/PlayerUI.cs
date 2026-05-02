using TMPro;
using UnityEngine;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerGunText;
    [SerializeField] GameObject indicatorObject;
    [SerializeField] Canvas PlayerUIcanvas;
    [SerializeField] GameObject Cross;

    private void Update()
    {
        UpdateCross();
    }

    void UpdateCross()
    {
        Cross.SetActive(PlayerComponents.Instance.InputManager.CurrentGunState != InputManager.GunState.ADS);
    }

    public Indicator CreateIndicator()
    {
        GameObject indicator = Instantiate(indicatorObject,PlayerUIcanvas.gameObject.transform);

        return indicator.GetComponentInChildren<Indicator>();
    }

    public void UpdateGunUI(int CurrentMag, int TotalMag)
    {
        PlayerGunText.text = $"{CurrentMag} / {TotalMag}";
    }

}