using UnityEngine;

public class SpringFollow : MonoBehaviour
{
    public Transform Target;
    public float Frequency, Damping;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 vel = _rb.linearVelocity;
        transform.position = Spring(transform.position, Target.position,ref vel,  Damping, Frequency, Time.deltaTime);
        _rb.linearVelocity = vel;
    }

    public static Vector3 Spring(Vector3 currentPosition, Vector3 targetPosition, ref Vector3 velocity,float dampingRatio, float naturalFrequency, float deltaTime)
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
