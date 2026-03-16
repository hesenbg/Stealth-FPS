using UnityEngine;

public  class MathFunc
{

    private Vector3 xp; // previous input
    private Vector3 y, yd; // state variables
    private float k1, k2, k3; // dynamics constants
    const float PI = Mathf.PI;

    public void SecondOrderDynamics(float f, float z, float r, Vector3 x0)
    {
        // compute constants
        k1 = z / (PI * f);
        k2 = 1 / ((2 * PI * f) * (2 * PI * f));
        k3 = r * z / (2 * PI * f);

        // initialize variables
        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }

    public Vector3 UpdatePos(float T, Vector3 x, Vector3 xd)
    {
        if (xd == null)
        { // estimate velocity
            xd = (x - xp) / T;
            xp = x;
        }

        float k2_stable = Mathf.Max(k2, T * T / 2 + T * k1 / 2, T * k1); // clamp k2 to guarantee stability without jitter
        y = y + T * yd; // integrate position by velocity
        yd = yd + T * (x + k3 * xd - y - k1 * yd) / k2_stable; // integrate velocity by acceleration

        return y;
    }

}
