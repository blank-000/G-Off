using UnityEngine;


public static class UtilsMaths
{
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }



    public static Quaternion Multiply(Quaternion input, float scalar)
    {

        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }


    public static bool AlmostZero(Vector3 vector, float tolerance = 0.01f)
    {
        return vector.sqrMagnitude < tolerance * tolerance;
    }
}