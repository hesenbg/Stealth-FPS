using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    RawImage image;

    private void Start()
    {
        image = GetComponent<RawImage>();
    }

    private void Update()
    {
        //image.material.SetFloat("Width",)
    }
}