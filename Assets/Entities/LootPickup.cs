using UnityEngine;
public class LootPickup : MonoBehaviour
{
    public enum PickUpType { Ammo, Health , FlashNade, SmokeNade, DistractionNade}

    public PickUpType pickup;

    public int amount;

    ThrowAbleLogic throwAbleLogic;

    ShootLogic shootLogic;

    HealthManager healthManager;

    [SerializeField] LayerMask PlayerMask;

    private void Start()
    {
        throwAbleLogic = PlayerComponents.Instance.ThrowAbleLogic;
        shootLogic = PlayerComponents.Instance.ShootLogic;
        healthManager = PlayerComponents.Instance.HealthManager;

        if (throwAbleLogic == null) Debug.LogWarning("LootPickup: ThrowAbleLogic is null");
        if (shootLogic == null) Debug.LogWarning("LootPickup: ShootLogic is null");
        if (healthManager == null) Debug.LogWarning("LootPickup: HealthManager is null");
    }

    private void AddLoot()
    {
        switch (pickup)
        {
            case PickUpType.Ammo:
                shootLogic.AddAmmo(amount);
                break;

            case PickUpType.Health:
                LootableItemInventory.Instance.IncreaseSynringeCount();
                LootableItemInventory.Instance.IncreaseSynringeCount();
                break;
            case PickUpType.FlashNade:
                throwAbleLogic.IncreaseFlash(amount);
                break;

            case PickUpType.SmokeNade:
                throwAbleLogic.IncreaseSmoke(amount);
                break;

            case PickUpType DistractionNade:
                throwAbleLogic.IncreaseDistraction(amount);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((PlayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            AddLoot();
            Destroy(gameObject);
        }
    }
}
