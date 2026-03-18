using UnityEngine;
using System;
public static class MathFunc
{
    public static float ForwardSight(Vector3 direction, Vector3 forward)
    {
        float ForwardDot = Vector3.Dot(direction, forward);
        return Mathf.Acos(ForwardDot) * Mathf.Rad2Deg;
    }

    public static float UpSight(Vector3 direction, Vector3 up)
    {
        float UpDot = Vector3.Dot(direction, up);
        return Mathf.Acos(UpDot) * Mathf.Rad2Deg;
    }

    public static float RightSight(Vector3 direction, Vector3 right)
    {
        float rightDot = Vector3.Dot(direction, right);
        return Mathf.Acos(rightDot) * Mathf.Rad2Deg;
    }

    public struct SODState
    {
        public Vector3 xp;      // previous input
        public Vector3 y;       // output position
        public Vector3 yd;      // output velocity
        public float k1, k2, k3;
    }

    public static SODState SODCreate(float f, float z, float r, Vector3 x0)
    {
        SODState s;
        s.k1 = z / (Mathf.PI * f);
        s.k2 = 1f / ((2f * Mathf.PI * f) * (2f * Mathf.PI * f));
        s.k3 = r * z / (2f * Mathf.PI * f);
        s.xp = x0;
        s.y = x0;
        s.yd = Vector3.zero;
        return s;
    }

    public static Vector3 SODUpdate(ref SODState state, float T, Vector3 x, Vector3? xd = null)
    {
        Vector3 vel = xd ?? (x - state.xp) / T;   // estimate velocity if not supplied
        if (xd == null) state.xp = x;

        // Clamp k2 to guarantee stability without jitter
        float k2Stable = Mathf.Max(state.k2,
                        Mathf.Max(T * T / 2f + T * state.k1 / 2f,
                        T * state.k1));

        state.y += T * state.yd;
        state.yd += T * (x + state.k3 * vel - state.y - state.k1 * state.yd) / k2Stable;
        return state.y;
    }

    public static void Lerp(Vector3 start, Vector3 destination, float speed, float threshold, Action<Vector3> setter)
    {
        if ((destination - start).sqrMagnitude <= threshold) return;
        setter(Vector3.Lerp(start, destination, speed * Time.deltaTime));
    }

    public static void Lerp(Vector2 start, Vector2 destination, float speed, float threshold, Action<Vector2> setter)
    {
        if ((destination - start).sqrMagnitude <= threshold) return;
        setter(Vector2.Lerp(start, destination, speed * Time.deltaTime));
    }

    public static void Lerp(float start, float destination, float speed, float threshold, Action<float> setter)
    {
        if (Mathf.Abs(destination - start) <= threshold) return;
        setter(Mathf.Lerp(start, destination, speed * Time.deltaTime));
    }
}