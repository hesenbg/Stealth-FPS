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

    private void Update()
    {
        if (Input.GetKeyDown(toggleLookKey))
            lookEnabled = !lookEnabled;

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
            MouseX = 0f;
            MouseY = 0f;
        }

        ApplyRotation();
    }

    void GetMouseCoordinates()
    {
        MouseX = Input.GetAxisRaw("Mouse X") * MouseSensitivity;
        MouseY = Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
    }

    void UpdateRotation()
    {
        PlayerTransform.Rotate(Vector3.up * MouseX);
        Xrotation -= MouseY;
        Xrotation = Mathf.Clamp(Xrotation, -90f, 90f);
        TargetLocation.x = Xrotation;
    }

    void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(TargetLocation);
    }
}