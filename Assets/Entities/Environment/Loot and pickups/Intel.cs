using UnityEngine;

public class Intel : MonoBehaviour, Interactable
{
    public void OnInteract()
    {
        LootableItemInventory.Instance.IncreaseIntelCount();
        Destroy(gameObject);
    }
}
