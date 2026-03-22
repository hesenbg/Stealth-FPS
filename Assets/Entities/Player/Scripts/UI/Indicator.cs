using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    RawImage image;

    private void Start()
    {
        image = GetComponentInChildren<RawImage>();
    }

    public void UpdateIndicator(float AwarenessValue, float max)
    {
        image.material.SetFloat("Width", AwarenessValue/max);
        if(AwarenessValue < 0.1f)
        {
            image.material.SetFloat("Height", 0f);
        }
        else
        {
            image.material.SetFloat("Height", 1f);
        }
    }
}