using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MouseX;
    public float MouseY;

    [SerializeField] float MouseSensitivity;
    [SerializeField] Transform PlayerTransform;

    [Header("Input Control")]
    [SerializeField] KeyCode toggleLookKey = KeyCode.Tab;
    bool lookEnabled = true;

    float Xrotation;
    Vector3 TargetLocation;

    // recoil system
    float recoilX;
    float recoilVelocity;
    [SerializeField] float recoilKick;
    [SerializeField] float recoilReturnSpeed;

    private void Update()
    {
        // Toggle look on/off
        if (Input.GetKeyDown(toggleLookKey))
            lookEnabled = !lookEnabled;

        // Cursor handling based on state
        if (lookEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GetMouseCoordinates();
            UpdateRotation();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // prevent leftover input affecting rotation
            MouseX = 0f;
            MouseY = 0f;
        }

        // Smooth recoil return
        recoilX = Mathf.SmoothDamp(recoilX, 0f, ref recoilVelocity, 1f / recoilReturnSpeed);

        ApplyRotation();
    }

    void GetMouseCoordinates()
    {
        MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
    }

    void UpdateRotation()
    {
        PlayerTransform.Rotate(Vector3.up * MouseX);

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
        recoilX -= Kick;
    }
}
