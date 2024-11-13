using UnityEngine;



public class Move : MonoBehaviour
{
    #region INPUT

    public float DebugLineScale = 10f;
    Camera _cam;
    Vector3 _moveInput;
    public bool HasControls;

    public void OnMove(Vector2 input)
    {
        if (!isControlsOn) return;

        // _moveInput = new Vector3(input.x, 0F, input.y);

        //TODO get project the input to be relative to the transform forward/back ward, and

        _moveInput = transform.right * input.x + transform.forward * input.y;



        // Debug.DrawLine(transform.position, transform.position + transform.right * input.x * DebugLineScale, Color.red);
        // Debug.DrawLine(transform.position, transform.position + transform.forward * input.y * DebugLineScale, Color.blue);


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
        Debug.DrawLine(transform.position, transform.position + transform.right * _moveInput.x * DebugLineScale, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.forward * _moveInput.z * DebugLineScale, Color.blue);
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