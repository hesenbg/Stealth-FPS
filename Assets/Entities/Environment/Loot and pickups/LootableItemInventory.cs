using UnityEngine;
public class LootableItemInventory : MonoBehaviour
{
    public static LootableItemInventory Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int keyCount {  get; private set; }

    public int IntelCount { get; private set; }

    public int HealSynringeCount { get; private set; }


    public void IncreaseKeyCount()
    {
        keyCount++;
    }

    public void IncreaseIntelCount()
    {
        IntelCount++;
    }

    public void IncreaseSynringeCount()
    {
        HealSynringeCount++;
    }
    public void DecreaseKeyCount()
    {
        keyCount--;
    }

    public void DecreaseSyringeCount()
    {
        HealSynringeCount--;
    }
}
