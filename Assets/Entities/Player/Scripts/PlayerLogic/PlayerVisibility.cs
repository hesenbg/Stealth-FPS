using UnityEngine;
public class PlayerVisibility : MonoBehaviour
{
    public bool visible = false;
    public bool IsSmoked;

    GameObject[] LightSources;

    private void Start()
    {
    }
    private void Update()
    {
        IsPlayerVisible();
    }
    void  IsPlayerVisible()
    {
        LightSources = GameObject.FindGameObjectsWithTag("LightSources");

        foreach (GameObject LightSource in LightSources)
        {
            LightData CurrentData = LightSource.gameObject.GetComponent<LightData>();
            if (CurrentData.IsPlayerIlluminated && !IsSmoked)
            {
                visible = true;
                return;
            }
        }
        visible = false;
    }
}
