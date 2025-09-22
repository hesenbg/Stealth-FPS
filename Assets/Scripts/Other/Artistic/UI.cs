using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("UI componenets")]
    [SerializeField] TextMeshProUGUI grenadeCount;
    [Header("Gameobjects")]
    [SerializeField] ThrowGrenade ThrowGrenade;


    private void Update()
    {
        grenadeCount.text = $"{ThrowGrenade.ExplosiveCount}";
    }
}
