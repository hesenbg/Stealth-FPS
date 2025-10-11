using Unity.Mathematics;
using UnityEngine;

public class ProceduralArms : MonoBehaviour
{
    [SerializeField] float Sway;
    [SerializeField] float Speed;

    [SerializeField] Vector3 MouseMovement;


    void GetInput()
    {
        MouseMovement.x = Input.GetAxisRaw("Mouse X") * Sway;
        MouseMovement.y = Input.GetAxisRaw("Mouse Y") * Sway;
    }

    void ApllySway()
    {
        Quaternion RotX = Quaternion.AngleAxis( -MouseMovement.y,Vector3.right);
        Quaternion RotY = Quaternion.AngleAxis(MouseMovement.x ,Vector3.up);

        Quaternion Target = RotX * RotY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Target,Speed*Time.deltaTime);

    }

    private void Update()
    {
        GetInput();

        ApllySway();
    }





}
