using UnityEngine;

public class ArcMovement : MonoBehaviour
{
    private Vector3 launchVelocity; // Initial velocity of the object.
    private Vector3 target;       // Optional: Target to aim at.
    [SerializeField] private bool useGravity = true; // Enable or disable gravity for the arc.

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (useGravity)
        {
            rb.useGravity = true; // Ensure gravity is enabled for the Rigidbody.
        }

        Launch();
    }

    public void Launch()
    {
        target = transform.position + transform.forward * 5;
        launchVelocity = CalculateLaunchVelocity(target);


        rb.linearVelocity = launchVelocity; // Apply the calculated or specified velocity.
    }

    private Vector3 CalculateLaunchVelocity(Vector3 targetPosition)
    {
        Vector3 displacement = targetPosition - transform.position;
        float horizontalDistance = new Vector2(displacement.x, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        float gravity = Physics.gravity.magnitude;
        float angle = 45f * Mathf.Deg2Rad; // Launch angle in radians (adjustable if needed).

        // Solve for initial speed using the physics equation:
        // v^2 = g * d / sin(2 * angle)
        float speed = Mathf.Sqrt(gravity * horizontalDistance / Mathf.Sin(2 * angle));

        // Decompose velocity into horizontal and vertical components.
        float vx = speed * Mathf.Cos(angle);
        float vy = speed * Mathf.Sin(angle);

        Vector3 velocity = displacement.normalized * vx; // Horizontal direction.
        velocity.y = vy; // Vertical component.

        return velocity;
    }

    private void OnDrawGizmos()
    {
        // Visualize the trajectory in the Editor.
        if (launchVelocity != Vector3.zero)
        {
            Gizmos.color = Color.cyan;
            Vector3 position = transform.position;
            Vector3 velocity = launchVelocity;

            for (int i = 0; i < 10; i++)
            {
                Gizmos.DrawSphere(position, 0.1f);
                velocity += Physics.gravity * Time.fixedDeltaTime; // Update velocity with gravity.
                position += velocity * Time.fixedDeltaTime;       // Update position.
            }
        }
    }
}
