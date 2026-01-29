using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MouseX;
    public float MouseY;

    [SerializeField] float MouseSensitivity;
    [SerializeField] Transform PlayerTransform;

    float Xrotation;
    Vector3 TargetLocation;

    // recoil system
    float recoilX;
    float recoilVelocity;
    [SerializeField] float recoilKick;
    [SerializeField] float recoilReturnSpeed;

    private void Start()
    {
    }

    private void Update()
    {
        // Lock cursor to center and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GetMouseCoordinates();
        UpdateRotation();

        // Smooth recoil return
        recoilX = Mathf.SmoothDamp(recoilX, 0f, ref recoilVelocity, 1f / recoilReturnSpeed);

        ApplyRotation();

        // Optional: Toggle lock state for debugging
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void GetMouseCoordinates()
    {
        MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
    }

    void UpdateRotation()
    {
        // horizontal rotation (yaw)
        PlayerTransform.Rotate(Vector3.up * MouseX);

        // vertical rotation (pitch)
        Xrotation -= MouseY;
        Xrotation = Mathf.Clamp(Xrotation, -90f, 90f);

        TargetLocation.x = Xrotation + recoilX;
    }

    void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(TargetLocation);
    }

    public void ApplyRecoilMotion(float Kick)
    {
        // Typical vertical recoil kick is negative for upward movement
        recoilX -= Kick;
    }
}