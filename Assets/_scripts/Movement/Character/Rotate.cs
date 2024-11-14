using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Movement Settings
    [SerializeField] MoveSettings _settings;

    Rigidbody _RB;
    Camera _cam;

    Vector3 _moveInput = Vector3.zero;
    [HideInInspector] public Quaternion TargetRotation;

    public void OnMove(Vector2 input)
    {
        _moveInput = transform.right * input.x + transform.forward * input.y;

        _moveInput.Normalize();
    }


    void Awake()
    {
        _cam = Camera.main;
        _RB = GetComponent<Rigidbody>();
        if (_settings.input != null)
        {
            _settings.input.moveEvent += OnMove;
        }
    }


    void FixedUpdate()
    {
        RotateToInput();
        UpdateUprightForce();
    }

    void RotateToInput()
    {
        Vector3 upDir = transform.up;
        Vector3 forwardDir;


        // Check if the input vector is almost zero
        if (XMath.AlmostZero(_moveInput))
        {
            // If no movement input, retain current forward direction but align it with the ground
            forwardDir = Vector3.ProjectOnPlane(transform.forward, upDir);
        }
        else
        {
            // Calculate forward direction based on movement input vector
            forwardDir = Vector3.ProjectOnPlane(_moveInput, upDir);
        }

        // when the Up Axis is Paralel to the ground (Finished slope corrections)
        if (!XMath.AlmostZero(forwardDir))
        {
            Quaternion rotationToFace = Quaternion.LookRotation(forwardDir, upDir.normalized);

            // Apply rotation
            TargetRotation = rotationToFace;
        }
    }

    void UpdateUprightForce()
    {
        Quaternion characterCurrent = transform.rotation;
        Quaternion toGoal = XMath.ShortestRotation(TargetRotation, characterCurrent);

        // Convert quaternion to angle-axis representation
        toGoal.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);

        // Normalize the rotation axis
        if (rotAxis != Vector3.zero)
        {
            rotAxis.Normalize();

            // Convert rotation degrees to radians
            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            // Calculate the spring torque based on the angle difference
            Vector3 springTorque = rotAxis * (rotRadians * _settings.SpringStrength);

            // Calculate the damping torque based on the current angular velocity
            Vector3 dampingTorque = -_RB.angularVelocity * _settings.SpringDamper;

            // Apply the total torque to the rigidbody, scaled by the mass
            _RB.AddTorque((springTorque + dampingTorque) * _RB.mass);
        }
    }

}
