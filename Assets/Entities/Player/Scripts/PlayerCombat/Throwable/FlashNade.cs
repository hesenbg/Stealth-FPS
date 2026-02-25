using UnityEngine;

public class FlashNade : BaseNade
{
    public override void ExecuteNadeLogic()
    {
        Debug.Log("Flash Nade Exloded");
        Destroy(gameObject);
    }
}
