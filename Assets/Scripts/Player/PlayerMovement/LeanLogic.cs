using UnityEngine;
public class LeanLogic : MonoBehaviour
{
    [SerializeField] float RotationSpeed = 100f;
    [SerializeField] float MaxLean = 15f;        
    private float currentZ = 0f; 

    public bool IsLeaning= false;
    [SerializeField] bool CanLean;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            CanLean = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            CanLean = true;
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && CanLean)
        {
            // Lean right
            currentZ = Mathf.MoveTowards(currentZ, MaxLean, RotationSpeed * Time.deltaTime);
            IsLeaning = true;
        }
        else if (Input.GetKey(KeyCode.E) && CanLean)
        {
            // Lean left
            currentZ = Mathf.MoveTowards(currentZ, -MaxLean, RotationSpeed * Time.deltaTime);
            IsLeaning = true;
        }
        else
        {
            IsLeaning = false;
            currentZ = Mathf.MoveTowards(currentZ, 0f, RotationSpeed * Time.deltaTime);
        }

        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            currentZ
        );
    }
}

