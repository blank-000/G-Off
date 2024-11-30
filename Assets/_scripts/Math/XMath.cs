using Unity.VisualScripting;
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

    public static bool PracticallyEqual(Quaternion a, Quaternion b)
    {
        var tolerance = 0.001f;
        return
            Mathf.Abs(a.x - b.x) < tolerance &&
            Mathf.Abs(a.y - b.y) < tolerance &&
            Mathf.Abs(a.z - b.z) < tolerance &&
            Mathf.Abs(a.w - b.w) < tolerance;
    }
    public static Quaternion FILerp(Quaternion a, Quaternion b, float slope)
    {
        // Using Slerp (spherical linear interpolation)
        float t = Mathf.Exp(-slope * Time.deltaTime); // Smoothing factor
        return Quaternion.Slerp(a, b, t);
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

    public static Vector3 Spring(Vector3 currentPosition, Vector3 targetPosition, ref Vector3 velocity, float dampingRatio, float naturalFrequency, float deltaTime)
    {
        // Calculate the spring constants
        float omega = naturalFrequency;
        float zeta = dampingRatio;
        float omegaSquared = omega * omega;

        // Damping coefficient based on zeta and omega
        float dampingCoefficient = 2f * zeta * omega;

        // Compute the acceleration of the spring using the differential equation
        Vector3 displacement = currentPosition - targetPosition;
        Vector3 acceleration = -omegaSquared * displacement - dampingCoefficient * velocity;

        // Integrate velocity and position
        velocity += acceleration * deltaTime;
        Vector3 newPosition = currentPosition + velocity * deltaTime;

        return newPosition;
    }

}