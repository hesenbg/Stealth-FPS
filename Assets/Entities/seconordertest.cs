using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class seconordertest : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("SOD")]
    [SerializeField] float sodFrequency = 2f;
    [SerializeField] float sodDamping = 1f;
    [SerializeField] float sodResponse = 0f;
    [SerializeField] float speed =1f;
    [SerializeField]

    MathFunc.SODState state;

    void Start()
    {
        state = MathFunc.SODCreate(sodFrequency, sodDamping, sodResponse, transform.position);
    }

    void Update()
    {
    }
}
