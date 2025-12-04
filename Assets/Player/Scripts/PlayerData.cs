using Unity.VisualScripting;
using UnityEngine;

public static class PlayerData
{
    public static PlayerMovement movement;
    public static AnimationLogic animationLogic;
    public static ShootLogic shootLogic;
    public static HealthManager healthManager;
    public static GroundCheck groundCheck;

    

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

    // GroundCheck
    public static void SetGroundCheck(GroundCheck value)
    {
        groundCheck = value;
    }

    public static GroundCheck GetGroundCheck()
    {
        return groundCheck;
    }
}
