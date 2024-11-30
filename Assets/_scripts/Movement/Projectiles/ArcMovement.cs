using Unity.VisualScripting;
using UnityEngine;

public class ArcMovement : MonoBehaviour
{
    private Vector3 launchVelocity; // Initial velocity of the object.
    private Vector3 target;         // Optional: Target to aim at.
    [SerializeField] private bool useGravity = true; // Enable or disable gravity for the arc.

    private Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        if (useGravity)
        {
            rb.useGravity = false;
        }
        Launch();
    }
    Vector3 gravity;
    public void Launch()
    {
        // gravity = WorldStateManager.Instance.GravityAxis;
        target = transform.position + transform.forward * 6;
        launchVelocity = CalculateLaunchVelocity(target);

        rb.linearVelocity = launchVelocity; // Apply the calculated or specified velocity.
    }
    private Vector3 CalculateLaunchVelocity(Vector3 targetPosition)
    {
        Vector3 displacement = targetPosition - transform.position;

        // Split displacement into horizontal and vertical components relative to gravity.
        Vector3 gravityDirection = gravity.normalized;
        float gravityMagnitude = gravity.magnitude;

        float verticalDisplacement = Vector3.Dot(displacement, -gravityDirection); // Along the gravity axis.
        Vector3 horizontalDisplacement = Vector3.ProjectOnPlane(displacement, gravityDirection);

        float horizontalDistance = horizontalDisplacement.magnitude;

        // Launch angle (can be adjusted if needed)
        float angle = 20f * Mathf.Deg2Rad;

        // Calculate launch speed using projectile motion physics:
        // v^2 = g * d / sin(2 * angle)
        float speed = Mathf.Sqrt(10 * horizontalDistance / Mathf.Sin(2 * angle));

        // Decompose launch velocity into components.
        float vx = speed * Mathf.Cos(angle);
        float vy = speed * Mathf.Sin(angle);

        // Combine velocity components into a single vector.
        Vector3 velocity = horizontalDisplacement.normalized * vx; // Horizontal component.
        velocity += -gravityDirection * vy;                        // Vertical component along gravity.

        return velocity;
    }
    void Update()
    {
        rb.AddForce(gravity * 500 * rb.mass * Time.deltaTime);
    }


}
