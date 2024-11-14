using UnityEngine;


public static class XMath
{

    // returns a the shortest rotation between a and b
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }

    // scales a quaternion
    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    // check if a vector is close enough to zero for all practical purposes
    public static bool AlmostZero(Vector3 vector, float tolerance = 0.01f)
    {
        return vector.sqrMagnitude < tolerance * tolerance;
    }

    // frame independent lerp smoothing for positions
    public static Vector3 FILerp(Vector3 a, Vector3 b, float slope)
    {
        return b + (a - b) * Mathf.Exp(-slope * Time.deltaTime);
    }

    // frame independent lerp smoothing for positions
    public static float FILerp(float a, float b, float slope)
    {
        return b + (a - b) * Mathf.Exp(-slope * Time.deltaTime);
    }

}