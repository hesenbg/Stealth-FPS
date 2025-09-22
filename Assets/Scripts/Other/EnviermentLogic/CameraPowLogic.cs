using UnityEngine;
public class CameraPowLogic : MonoBehaviour
{
    public float MouseX;
    public float MouseY;

    [SerializeField] float MouseSensitivity;

    [SerializeField] Transform PlayerTransform;

    float Xrotation;
    void GetMouseCoordinates()
    {
        MouseX = Input.GetAxis("Mouse X")*MouseSensitivity*Time.deltaTime;
        MouseY = Input.GetAxis("Mouse Y")*MouseSensitivity*Time.deltaTime;
    }
    void UpdateRotation()
    {
        PlayerTransform.Rotate(Vector3.up*MouseX);

        Xrotation-=MouseY;
        Xrotation= Mathf.Clamp(Xrotation, -90f, 90f);

        transform.localRotation= Quaternion.Euler(Xrotation,0,0);   
    }
    private void Update()
    {
        GetMouseCoordinates();
        UpdateRotation();
    }
}
