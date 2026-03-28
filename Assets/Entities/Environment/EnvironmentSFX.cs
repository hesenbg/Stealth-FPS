using UnityEngine;

public class EnvironmentSFX : MonoBehaviour
{
    public static EnvironmentSFX Instance;

    private void Awake()
    {
        Instance = this;
    }



}
