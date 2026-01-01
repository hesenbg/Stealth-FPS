using UnityEngine;
public class GunStateMachine : StateManager<GunStateMachine.GunState>
{
    public enum GunState {Idle, Reload, Shoot, Blocked}

    [SerializeField] float ShootDelay;
    [SerializeField] BulletHoleBehaviour BulletTrace;
    [SerializeField] int MagazineSize;
    [SerializeField] int TotalAmmo;
    [SerializeField] int CurrentAmmo;
    [SerializeField] float ReloadTime;
    [SerializeField] bool IsShootable;
    [SerializeField] float ShootCooldown;

    private GunContext Context;

    private void Awake()
    {
         Context = new GunContext(ShootDelay,BulletTrace, MagazineSize,
             TotalAmmo, CurrentAmmo, ReloadTime,IsShootable,ShootCooldown);

        AddStates();
    }

    void AddStates()
    {
        States.Add(GunState.Idle, new GunIdle(Context));
        States.Add(GunState.Reload, new GunReload(Context));
        States.Add(GunState.Shoot, new GunShoot(Context));
        States.Add(GunState.Blocked, new GunBlocked(Context));

        // Set initial state
        CurrentState = States[GunState.Idle];
    }
}