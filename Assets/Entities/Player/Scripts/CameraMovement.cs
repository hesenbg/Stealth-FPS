using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float XAmount = 0.02f;
    [SerializeField] float YAmount = 0.02f;
    [SerializeField] float ZAmount = 0.02f;
    [SerializeField] float SpeedEffectiveness = 1f;
    [SerializeField] float Frequency = 6f;
    [SerializeField] float InterpolationSpeed = 10f;
    [SerializeField] GameObject mesh;
    [SerializeField] float MeshEffectivnes;
    [SerializeField] float CamEffectTivnes;
    float time;
    Vector3 originalPos;
    Vector3 originalMeshPos;

    private void Start()
    {
        originalPos = transform.localPosition;
        originalMeshPos = mesh.transform.localPosition;
    }

    private void Update()
    {
        float speed = PlayerComponents.Instance.Movement.CurrentVelocity.magnitude;

        if (speed > 0.01f)
            time += Time.deltaTime * Frequency * speed;
        else
            time = Mathf.Lerp(time, 0f, Time.deltaTime * InterpolationSpeed);

        float sin = Mathf.Sin(time);
        float cos = Mathf.Cos(time);

        Vector3 offset = speed > 0.01f
            ? new Vector3(sin * XAmount, sin * YAmount, sin * ZAmount) * SpeedEffectiveness
            : Vector3.zero;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            originalPos + offset*CamEffectTivnes,
            Time.deltaTime * InterpolationSpeed
        );

        mesh.transform.localPosition = Vector3.Lerp(
            mesh.transform.localPosition,
            originalMeshPos + offset * MeshEffectivnes,
            Time.deltaTime * InterpolationSpeed
        );
    }
}