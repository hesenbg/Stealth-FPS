using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI PlayerGunText;
    [SerializeField] RawImage indicatorImage;
    [SerializeField] GameObject indicatorObject;

    private void Awake()
    {
        Sight.TargetEnterSight += OnTargetEnterSight;
    }



    private void OnTargetEnterSight(object sender, EventArgs e)
    {
        Instantiate(indicatorObject, transform);
    }

    public void UpdateGunUI(int CurrentMag, int TotalMag)
    {
        PlayerGunText.text = $"{CurrentMag} / {TotalMag}";
    }

}