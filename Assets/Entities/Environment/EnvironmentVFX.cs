using UnityEngine;

public class EnvironmentVFX : MonoBehaviour
{
    public static EnvironmentVFX Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject LightBulbVFX;

}
