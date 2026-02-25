using UnityEngine;

public class SmokeNade : BaseNade
{
    public override void ExecuteNadeLogic()
    {
        Debug.Log("Smoke nade opened");
        Destroy(gameObject);
    }
}
