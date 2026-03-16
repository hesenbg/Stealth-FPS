using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public static PlayerParameters instance;

    public float PlayerVisibility;

    public float EnemyAwareness;

    

    private void Awake()
    {
        instance = this;

        Sight.TargetSuspiciousSight += OnTargetSuspiciousSight;
        Sight.TargetEnterSight += OnTargetEnterSight;
        Sight.TargetFullySeen += OnTargetFullySeen;
        Sight.TargetoutSight += OnTargetoutSight;
    }

    private void OnTargetoutSight(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnTargetFullySeen(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnTargetEnterSight(object sender, System.EventArgs e)
    {

    }

    private void OnTargetSuspiciousSight(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
