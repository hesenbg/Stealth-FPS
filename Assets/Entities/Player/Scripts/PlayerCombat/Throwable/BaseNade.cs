using System;
using UnityEngine;

public abstract class BaseNade : MonoBehaviour
{
    public enum NadeThrowType { Long, Short }

    public NadeThrowType ThrowType;
    public float FuseTimer;
    public float EffectRadius;
    public float NadeEffectDuration;

    [SerializeField] public float ShortThrowForce;
    [SerializeField] private float Damping;
    [SerializeField] private float LongThrowForce;
    [SerializeField] private ParticleSystem NadeEffect;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask GroundLayer;

    protected float CurrFuseTimer;

    private bool hasActivated = false;
    private SphereCollider NadeCollider;

    private event EventHandler NadeActivated;
    private event EventHandler NadeDisabled;
    private event EventHandler TouchGround;
    private event EventHandler TimerEnd;

    private void Awake()
    {
        Transform cam = PlayerComponents.Instance.MainCamera.transform;
        rb.linearDamping = Damping;

        Vector3 throwDir = ThrowType == NadeThrowType.Long
            ? cam.forward.normalized
            : (cam.forward + cam.up).normalized;

        float throwForce = ThrowType == NadeThrowType.Long ? LongThrowForce : ShortThrowForce;
        rb.AddForce(throwDir * throwForce, ForceMode.VelocityChange);
    }

    private void Start()
    {
        NadeCollider = GetComponent<SphereCollider>();

        NadeActivated += OnNadeActivated;
        NadeDisabled += OnNadeDeactivated;
        TouchGround += OnTouchGround;

        Init();
    }

    private void Update()
    {
        if (CurrFuseTimer < FuseTimer)
        {
            CurrFuseTimer += Time.deltaTime;
        }
        else if (!hasActivated)
        {
            NadeActivated?.Invoke(this, EventArgs.Empty);
            hasActivated = true;
        }
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
        rb.isKinematic = true;
        NadeCollider.enabled = false;
    }

    public abstract void Init();
    public abstract void OnTouchGround(object sender, EventArgs e);
    public abstract void OnNadeActivated(object sender, EventArgs e);
    public abstract void OnNadeDeactivated(object sender, EventArgs e);
}