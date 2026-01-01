using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // events
    //[Header("Events")]
    // combat
    public event EventHandler OnReloadInput;
    public event EventHandler OnShootInput;
    public event EventHandler OnADS;


    // keys
    [Header("Movement")]
    [SerializeField] KeyCode MoveForward;
    [SerializeField] KeyCode MoveBackward;
    [SerializeField] KeyCode MoveLeft;
    [SerializeField] KeyCode MoveRight;
    [SerializeField] KeyCode Jump;
    [SerializeField] KeyCode Walk;

    [Header("Combat")]
    [SerializeField] MouseButton Shoot;
    [SerializeField] KeyCode Reload;
    [SerializeField] MouseButton ADS;

    [Header("Gadgets")]
    [SerializeField] KeyCode Throw;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {

    }
}