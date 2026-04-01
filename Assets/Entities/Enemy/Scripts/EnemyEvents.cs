using System;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public event EventHandler SuspiciosEvent;

    public event EventHandler PlayerSeen;

    public void FireSusEvent()
    {
        SuspiciosEvent?.Invoke(this, EventArgs.Empty);
    }
}