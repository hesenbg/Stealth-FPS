using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Lean : MonoBehaviour
{
    [SerializeField] float maxLeanAngle = 15f; // degrees
    [SerializeField] float speed = 8f;
    [SerializeField] Rig leanRig;

    private Quaternion initialLocalRot;
    private float currentAngle;
    private float targetAngle;
    private float targetWeight;

    void Start()
    {
        initialLocalRot = transform.localRotation;
    }

    void Update()
    {
        // Determine target
        targetAngle = 0f;
        targetWeight = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            targetAngle = maxLeanAngle;   // left
            targetWeight = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetAngle = -maxLeanAngle;  // right
            targetWeight = 1f;
        }

        // Smooth angle
        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            speed * Time.deltaTime
        );

        // Apply rotation ONLY on Z
        transform.localRotation =
            initialLocalRot * Quaternion.Euler(0f, 0f, currentAngle);

        // Smooth rig weight
        leanRig.weight = Mathf.Lerp(
            leanRig.weight,
            targetWeight,
            speed * Time.deltaTime
        );
    }
}
