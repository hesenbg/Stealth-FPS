using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI PlayerGunText;
    [SerializeField] GameObject indicatorObject;
    [SerializeField] Canvas PlayerUIcanvas;

    private void Awake()
    {
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