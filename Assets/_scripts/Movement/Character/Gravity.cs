using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float GravityForceScale;
    Vector3 _gravityDirection = Vector3.zero;
    Rigidbody _rb;
    Transform _cam;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main.transform;
    }

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.State);
    }


    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            _rb.linearVelocity = Vector3.zero;

            switch (state)
            {
                case WorldState.Light:
                    // x is up 
                    _gravityDirection = Vector3.forward;
                    break;
                case WorldState.Dark:
                    // y is up. this is the starting state
                    _gravityDirection = Vector3.down;
                    break;
            }
        }
    }

    public float ForwardOffsetDegreees = 45f;

    void FixedUpdate()
    {
        // _rb.AddForce(GravityForceScale * _gravityDirection * _rb.mass * Time.deltaTime, ForceMode.VelocityChange);


        RotateUp();
        // SpringRide();
    }

    // public float RideHeight = 1f;
    // public float Damping = 1f;
    // public float Frequency = 6f;
    // public LayerMask Ground;

    // Vector3 targetPosition;
    // void SpringRide()
    // {
    //     targetPosition = transform.position;
    //     if (Physics.Raycast(transform.position, _gravityDirection, out RaycastHit hitInfo, Ground))
    //     {
    //         targetPosition = hitInfo.point + transform.up * RideHeight;
    //         Debug.Log(targetPosition);
    //     }

    //     Debug.DrawLine(transform.position, transform.position + (_gravityDirection * RideHeight), Color.yellow);

    //     Vector3 vel = _rb.linearVelocity;
    //     transform.position = XMath.Spring(transform.position, targetPosition, ref vel, Damping, Frequency, Time.deltaTime);
    //     _rb.linearVelocity = vel;


    // }

    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(targetPosition, 0.1f);
    // }

    void RotateUp()
    {

        //Careful!! GPT Drivvel follows, Check and double check

        // Define your target forward and up directions
        Vector3 targetUp = -_gravityDirection.normalized;
        Vector3 offsetForward = Quaternion.AngleAxis(ForwardOffsetDegreees, targetUp) * _cam.forward;
        Vector3 targetForward = Vector3.ProjectOnPlane(offsetForward, targetUp);


        // Calculate the target rotation based on the new forward and up vectors
        Quaternion targetRotation = Quaternion.LookRotation(targetForward, targetUp);

        // Calculate the rotation difference
        Quaternion rotationDifference = targetRotation * Quaternion.Inverse(transform.rotation);

        // Convert the quaternion difference to an axis-angle representation
        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

        // Apply torque based on the angle and axis to gradually align both the up and forward directions
        if (angle > 0.01f) // Add a small threshold to prevent jitter
        {
            Vector3 torque = axis * angle * Mathf.Deg2Rad * 50f;
            _rb.AddTorque(torque, ForceMode.VelocityChange);
        }
    }
}
