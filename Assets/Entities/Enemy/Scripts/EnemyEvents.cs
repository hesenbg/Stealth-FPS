using System;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public event EventHandler SuspiciosEvent;

    public event EventHandler PlayerSeen;

    private void Start()
    {
        SuspiciosEvent += EnemyEvents_SuspiciosEvent;
    }

    private void EnemyEvents_SuspiciosEvent(object sender, EventArgs e)
    {
        Debug.Log("fired");
    }

    public void FireSusEvent()
    {
        SuspiciosEvent?.Invoke(this, EventArgs.Empty);
    }
}