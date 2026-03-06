using UnityEngine;
using UnityEngine.UI;

public class UIbuttonAnimationPlayer : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 10f;
    public bool loop = true;

    private Image imageComponent;
    private int index = 0;
    private float timer = 0f;

    void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f / framesPerSecond)
        {
            timer = 0f;
            index++;

            if (index >= frames.Length)
            {
                if (loop)
                {
                    index = 0;
                }
                else
                {
                    index = frames.Length - 1;
                    enabled = false;
                }
            }

            imageComponent.sprite = frames[index];
        }
    }
}
