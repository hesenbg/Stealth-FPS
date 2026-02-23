using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerGunText;

    public void UpdateGunUI(int CurrentMag, int TotalMag)
    {
        PlayerGunText.text = $"{CurrentMag} / {TotalMag}";
    }
}