using System;
using UnityEngine;

public abstract class BaseNade : MonoBehaviour
{
    public float FuseTimer;
    protected float CurrFuseTimer;
    public float EffectRadius;

    [SerializeField] float Damping;
    [SerializeField] ParticleSystem NadeEffect;
    [SerializeField] Rigidbody rb;
    [SerializeField] float LongThrowForce;
    [SerializeField] public float ShortThrowForce;
    [SerializeField] LayerMask GroundLayer;

    public enum NadeThrowType { Long, Short }
    public NadeThrowType ThrowType;

    event EventHandler NadeActivated;
    event EventHandler NadeDisabled;
    event EventHandler TouchGround;
    event EventHandler TimerEnd;
    SphereCollider NadeCollider;

    private void Awake()
    {
        Transform cam = PlayerComponents.Instance.MainCamera.transform;
        rb.linearDamping = Damping;

        Vector3 throwDir = ThrowType == NadeThrowType.Long
            ? cam.forward.normalized
            : (cam.forward + cam.up).normalized;

        float throwForce = ThrowType == NadeThrowType.Long ? LongThrowForce : ShortThrowForce;
        rb.AddForce(throwDir * throwForce, ForceMode.Impulse);
    }

    private void Start()
    {
        NadeActivated += OnNadeActivated;
        NadeDisabled += OnNadeDeactivated;
        TouchGround += OnTouchGround;
    }

    public void Update()
    {
        if (CurrFuseTimer < FuseTimer)
            CurrFuseTimer += Time.deltaTime;
        else
            NadeActivated?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((GroundLayer.value & (1 << collision.collider.gameObject.layer)) != 0)
        {
            TouchGround?.Invoke(this, EventArgs.Empty);
        }
    }

    protected void ExecuteNadeEffects()
    {
        if (NadeEffect == null) return;
        Instantiate(NadeEffect, transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }

    protected void DisableNadePhysics()
    {
        rb.linearVelocity = Vector3.zero;
        rb.gameObject.SetActive(false);
        NadeCollider.gameObject.SetActive(false);
    }

    public abstract void OnTouchGround(object sender, EventArgs e);
    public abstract void OnNadeActivated(object sender, EventArgs e);
    public abstract void OnNadeDeactivated(object sender, EventArgs e);
}