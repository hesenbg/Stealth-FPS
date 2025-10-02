using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] PlayerMovement movement;
    [SerializeField] ShootLogic shoot;

    public float TotalRecoil;

    public float ShootRecoil;   // recoil created when player shoots

    public float MoveRecoil;    // players movement can change recoil

    [SerializeField] float RecoilShotChange;
    [SerializeField] float MoveRecoilChange;
    [SerializeField] float MaxShootRecoil;
    [SerializeField] float MaxMoveRecoil;

    [SerializeField] float ShotRecoilResetDuration;
    float ShotRecoilResetDurationValue;

    float MoveRecoilIncrement;

    public float CalculateShotRecoil()
    {
        if (shoot.IsShooting && ShootRecoil<MaxMoveRecoil)
        {         
            ShootRecoil += RecoilShotChange;
        }
        else if (ShootRecoil !=0)
        {
            ShootRecoil -= RecoilShotChange*Time.deltaTime*ShotRecoilResetDuration;
        }

        return ShootRecoil;
    }

    public float CalculateMoveRecoil()
    {
        if (movement.IsMoving && MoveRecoil<MaxMoveRecoil)
        {
            MoveRecoil += MoveRecoilIncrement * Time.deltaTime;
        }

        else
        {
            MoveRecoil -= MoveRecoilChange*Time.deltaTime;
        }
        return MoveRecoil;
    }



}