using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public static class PlayerData
{
    static PlayerMovement movement;
    static AnimationLogic animationLogic;
    static ShootLogic shootLogic;
    static HealthManager healthManager;
    static Rig ADSrig;
    static Camera PlayerMainCamera;
    static WeaponPullLogic pullLogic;
    // pull logic
    public static void SetPlayerPullLogiv(WeaponPullLogic pull)
    {
        pullLogic = pull;
    }

    public static WeaponPullLogic GetPullLogic()
    {
        return pullLogic;
    }

    // plyaer camera
    public static void SetPlayerCam(Camera cam)
    {
        PlayerMainCamera = cam;
    }

    public static Camera GetCamera()
    {
        return PlayerMainCamera;
    }

    // ADS rig
    public static void SetADSrig(Rig rig)
    {
        ADSrig = rig;
    }

    public static Rig GetADSrig()
    {
        return ADSrig;
    }

    // Movement
    public static void SetMovement(PlayerMovement value)
    {
        movement = value;
    }

    public static PlayerMovement GetMovement()
    {
        return movement;
    }

    // AnimationLogic
    public static void SetAnimationLogic(AnimationLogic value)
    {
        animationLogic = value;
    }

    public static AnimationLogic GetAnimationLogic()
    {
        return animationLogic;
    }

    // ShootLogic
    public static void SetShootLogic(ShootLogic value)
    {
        shootLogic = value;
    }

    public static ShootLogic GetShootLogic()
    {
        return shootLogic;
    }

    // HealthManager
    public static void SetHealthManager(HealthManager value)
    {
        healthManager = value;
    }

    public static HealthManager GetHealthManager()
    {
        return healthManager;
    }

}
