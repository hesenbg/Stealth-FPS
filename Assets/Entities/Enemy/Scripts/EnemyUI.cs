using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// we need current state of the enemy and wheater if it is flashed or not

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Image EnemyUIimage;

    [SerializeField] List<Sprite> States;

    [SerializeField] Sprite FlashEffect;

    [SerializeField] Sprite StunEffect;

    private void Start()
    {

        EnemyUIimage = GetComponentInChildren<Image>();

        EnemyUIimage.sprite = States[0];
    }

    public bool IsEffected { get; private set; }

    public void FlashEffectUI()
    {
        IsEffected = true;
        EnemyUIimage.sprite = FlashEffect;
    }

    public void DeActiveFlashEffectUI()
    {
        IsEffected = false;
    }

    public void IdleUI()
    {
        if (IsEffected) return;
        EnemyUIimage.sprite = States[0];
    }

    public void SuspiciousUI()
    {
        if (IsEffected) return;
        EnemyUIimage.sprite = States[1];
    }

    public void AlarmedUI()
    {
        if (IsEffected) return;
        EnemyUIimage.sprite = States[2];
    }
}