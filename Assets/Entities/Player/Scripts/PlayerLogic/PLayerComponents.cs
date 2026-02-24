using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerComponents : MonoBehaviour
{
    // The static instance that other scripts will use
    public static PlayerComponents Instance { get;  set; }

    [Header("Core Systems")]
    [SerializeField] private MovementLogic movement;
    [SerializeField] private AnimationLogic animationLogic;
    [SerializeField] private ShootLogic shootLogic;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ThrowAbleLogic throwAbleLogic;

    [Header("Technical/Visual")]
    [SerializeField] private Rig combatAnimation;
    [SerializeField] private Camera playerMainCamera;
    [SerializeField] private WeaponWallBlock wallBlock;
    [SerializeField] private ADS ads;
    [SerializeField] private PlayerUI playerUI;

    // Getters
    public ThrowAbleLogic ThrowAbleLogic => throwAbleLogic;
    public PlayerUI PlayerUI => playerUI;
    public InputManager InputManager  => inputManager;
    public ADS ADS => ads;
    public MovementLogic Movement => movement;
    public WeaponWallBlock WallBlock => wallBlock;
    public AnimationLogic AnimationLogic => animationLogic;
    public ShootLogic ShootLogic => shootLogic;
    public HealthManager HealthManager => healthManager;
    public Rig CombatAnimation => combatAnimation;
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
    }
}