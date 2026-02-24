using UnityEngine;
abstract public class BaseNade : MonoBehaviour
{
    public  float FuseTimer;

    private float CurrFuseTimer;

    public float EffectRadius;

    [SerializeField] Mesh NadeMesh;

    [SerializeField] ParticleSystem NadeEffect;

    [SerializeField] public Rigidbody rb;

    [SerializeField] public float force;

    private void Awake()
    {
        Transform cam = PlayerComponents.Instance.MainCamera.transform;
        Debug.Log((cam.forward + cam.up));
        rb.AddForce((cam.forward + cam.up) * 5f, ForceMode.Impulse);
    }

    public void Update()
    {
        

        if (CurrFuseTimer < FuseTimer)
        {
            CurrFuseTimer +=Time.deltaTime;
        }
        else
        {
            ExecuteNadeLogic();

        }
    }

    private void ExecuteNadeEffects()
    {

    }

    abstract public void ExecuteNadeLogic();
}