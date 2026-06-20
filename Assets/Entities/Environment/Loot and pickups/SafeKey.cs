using UnityEngine;

public class SafeKey : MonoBehaviour, Interactable
{
    public void OnInteract()
    {
        LootableItemInventory.Instance.IncreaseKeyCount();
        Destroy(gameObject);
    }
}
