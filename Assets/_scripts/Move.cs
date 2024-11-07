using UnityEngine;



public class Move : MonoBehaviour
{
    #region INPUT


    Camera _cam;
    Vector3 _moveInput;
    public bool HasControls;

    public void OnMove(Vector2 input)
    {
        if (!isControlsOn) return;

        _moveInput = new Vector3(input.x, 0F, input.y);

        if (settings.isRelativeToCamera)
        {
            float angle = -_cam.transform.eulerAngles.y * Mathf.Deg2Rad;

            float rotatedX = _moveInput.x * Mathf.Cos(angle) - _moveInput.z * Mathf.Sin(angle);
            float rotatedZ = _moveInput.x * Mathf.Sin(angle) + _moveInput.z * Mathf.Cos(angle);

            _moveInput = new Vector3(rotatedX, 0f, rotatedZ);
        }
        _moveInput.Normalize();
    }

    //allows other scripts to toggle the move controls
    public void EnableControls()
    {
        isControlsOn = true;
    }
    public void DisableControls()
    {
        isControlsOn = false;
    }

    #endregion

    // settings
    [SerializeField] MoveSettings settings;

    // Ref
    Rigidbody _RB;

    // Var
    Vector3 GoalVel = Vector3.zero;
    bool isControlsOn;


    void Awake()
    {
        _cam = Camera.main;
        _RB = GetComponent<Rigidbody>();

        if (settings.input != null)
        {
            settings.input.moveEvent += OnMove;
            HasControls = true;
        }

        EnableControls();
    }


    void FixedUpdate()
    {
        MoveByInput();
    }


    void MoveByInput()
    {
        Vector3 unitVel = GoalVel.normalized;

        float velDot = Vector3.Dot(_moveInput, unitVel);

        float accel = settings.acceleration * settings.accelerationFactorFromDot.Evaluate(velDot);

        Vector3 goalVel = _moveInput * settings.maxSpeed;

        GoalVel = Vector3.MoveTowards(GoalVel, goalVel, accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (GoalVel - _RB.linearVelocity) / Time.fixedDeltaTime;

        float maxAccel = settings.maxAccelerationForce * settings.maxAccelerationFactorFromDot.Evaluate(velDot);

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        Vector3 forwardForce = Vector3.Scale(neededAccel * _RB.mass, settings.forceScale);

        _RB.AddForce(forwardForce);
    }

}
