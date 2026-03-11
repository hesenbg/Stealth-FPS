using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Sight : MonoBehaviour
{
    [SerializeField] float ForwardMax;
    [SerializeField] float Angle;

    [SerializeField] GameObject Target;

    [SerializeField] float ForwardMin;

    [Header("dots")]
    public float ForwardDot;

    public float RightDot;

    [Header("angles")]
    public float forwardCos;

    public float rightCos;

    
    public event EventHandler OnTargetinSIght;
    public event EventHandler OnTargetoutSIght;

    private void FixedUpdate()
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;

        ForwardDot = Vector3.Dot(direction, transform.forward);

        RightDot = Vector3.Dot(direction, transform.right);

        forwardCos = Mathf.Acos(ForwardDot)*Mathf.Rad2Deg;

        rightCos = Mathf.Acos(RightDot) * Mathf.Rad2Deg;

        if(rightCos>Angle && rightCos < Angle+90f && ForwardDot>ForwardMin && (Target.transform.position-transform.position).magnitude<ForwardMax )
        {
            OnTargetinSIght?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnTargetoutSIght?.Invoke(this, EventArgs.Empty);
        }
    }
}