using UnityEngine;
public class CameraPowLogic : MonoBehaviour
{
    public float MouseX;
    public float MouseY;

    [SerializeField] float MouseSensitivity;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] ShootLogic shoot;

    float Xrotation;

    Vector3 TargetLocation;

    // recoil system
    float recoilX;              // current recoil rotation
    float recoilVelocity;       // smoothing value for recoil
    [SerializeField] float recoilKick ;       // how much the camera jumps each shot
    [SerializeField] float recoilReturnSpeed ; // how fast recoil returns

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

        TargetLocation.x = Xrotation + recoilX; // add recoil effect
    }

    void ApllyRotation()
    {
        transform.localRotation = Quaternion.Euler(TargetLocation);
    }

    public void ApllyRecoilMoation(float Kick)
    {
        // add upward kick
        recoilX += Random.Range(-Kick,Kick);
    }

    private void Update()
    {
        GetMouseCoordinates();
        UpdateRotation();

        // Smooth recoil return
        recoilX = Mathf.SmoothDamp(recoilX, 0f, ref recoilVelocity, 1f / recoilReturnSpeed);

        ApllyRotation();
    }
}
