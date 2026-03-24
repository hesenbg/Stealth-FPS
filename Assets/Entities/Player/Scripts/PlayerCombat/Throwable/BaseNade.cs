using UnityEngine;
abstract public class BaseNade : MonoBehaviour
{
    public  float FuseTimer;

    private float CurrFuseTimer;

    public float EffectRadius;

    [SerializeField] Mesh NadeMesh;

    [SerializeField] float Damping;

    [SerializeField] ParticleSystem NadeEffect;

    [SerializeField]  Rigidbody rb;

    [SerializeField]  float LongThrowForce;
    [SerializeField] public float ShortThrowForce;

    public enum NadeThrowType { Long, Short}
    public NadeThrowType ThrowType;

    private void Awake()
    {
        Transform cam = PlayerComponents.Instance.MainCamera.transform;

        rb.linearDamping = Damping;

        if(ThrowType == NadeThrowType.Long)
            rb.AddForce((cam.forward).normalized * LongThrowForce, ForceMode.Impulse);
        else
            rb.AddForce((cam.forward+ cam.up).normalized*ShortThrowForce , ForceMode.Impulse);
    }

    public void Update()
    {
        if (CurrFuseTimer < FuseTimer)
        {
            CurrFuseTimer +=Time.deltaTime;
        }
        else
        {
            ExecuteNadeEffects();
            ExecuteNadeLogic();
        }
    }

    private void ExecuteNadeEffects()
    {
        if (NadeEffect == null)
            return;
        Instantiate(NadeEffect, transform.position, Quaternion.Euler(-90f,0f,0f));
    }

    abstract public void ExecuteNadeLogic();
}