using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float GravityForceScale;
    Vector3 _gravityDirection = Vector3.zero;
    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
                    _gravityDirection = Vector3.left;
                    break;
                case WorldState.MidTone:
                    // y is up
                    _gravityDirection = Vector3.down;
                    break;
                case WorldState.Dark:
                    // z is up
                    _gravityDirection = Vector3.back;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        _rb.AddForce(GravityForceScale * _gravityDirection * _rb.mass * Time.deltaTime, ForceMode.VelocityChange);

        //Careful GPT Drivvel follows, Check and double check

        // Calculate the rotation needed to align the local up with the gravity direction
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -_gravityDirection) * transform.rotation;

        // Calculate the rotation difference
        Quaternion rotationDifference = targetRotation * Quaternion.Inverse(transform.rotation);

        // Convert the quaternion difference to an axis-angle representation
        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

        // Apply torque based on the angle and axis to gradually align the up direction
        if (angle > 0.01f) // Add a small threshold to prevent jitter
        {
            Vector3 torque = axis * angle * Mathf.Deg2Rad * 50f;
            _rb.AddTorque(torque, ForceMode.VelocityChange);
        }
    }
}
