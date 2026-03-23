using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    RawImage image;
    public GameObject parent;

    private void Awake()
    {
        image = GetComponentInChildren<RawImage>();

        // Create a unique material instance for this indicator
        image.material = new Material(image.material);
    }

    public void UpdateIndicator(float AwarenessValue, float max, float Angle)
    {
        parent.transform.rotation = Quaternion.Euler(0f, 0f, Angle);

        image.material.SetFloat("_Width", AwarenessValue / max);

        if (AwarenessValue < 0.1f)
        {
            image.material.SetFloat("_Height", 0f);
        }
        else
        {
            image.material.SetFloat("_Height", 1f);
        }
    }
}