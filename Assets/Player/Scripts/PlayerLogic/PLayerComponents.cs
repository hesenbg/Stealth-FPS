using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerComponents : MonoBehaviour
{
    // The static instance that other scripts will use
    public static PlayerComponents Instance { get;  set; }

    [Header("Core Systems")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private AnimationLogic animationLogic;
    [SerializeField] private ShootLogic shootLogic;
    [SerializeField] private HealthManager healthManager;

    [Header("Technical/Visual")]
    [SerializeField] private Rig adsRig;
    [SerializeField] private Camera playerMainCamera;
    [SerializeField] private WeaponWallBlock WallBlock;

    // Getters
    public PlayerMovement Movement => movement;
    public AnimationLogic AnimationLogic => animationLogic;
    public ShootLogic ShootLogic => shootLogic;
    public HealthManager HealthManager => healthManager;
    public Rig ADSRig => adsRig;
    public Camera MainCamera => playerMainCamera;
    public WeaponWallBlock PullLogic => WallBlock;

    private void Awake()
    {
        // Check if an instance already exists to ensure only one "PlayerComponents" exists
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        // Optional: Keep the player between scene loads
        // DontDestroyOnLoad(gameObject);
    }
}