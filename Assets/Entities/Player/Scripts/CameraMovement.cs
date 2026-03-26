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
    float time;

    private void Update()
    {
        float speed = PlayerComponents.Instance.Movement.CurrentVelocity.magnitude;

        time += Time.deltaTime * Frequency * speed;

        float sin = Mathf.Sin(time);
        float cos = Mathf.Cos(time);

        Vector3 target = new Vector3(
            cos * XAmount,
            sin * YAmount,
            sin * ZAmount
        ) * SpeedEffectiveness;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            target,
            Time.deltaTime * InterpolationSpeed
        );

        mesh.transform.localPosition = Vector3.Lerp(
            mesh.transform.localPosition,
            target*MeshEffectivnes,
            Time.deltaTime * InterpolationSpeed
        );
    }
}