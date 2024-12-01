using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
    #region INPUT && Events

    Camera _cam;
    Vector3 _moveInput;
    Vector2 _rawInput;


    // recieve input 
    public void OnMove(Vector2 input)
    {
        if (!isControlsOn) return;
        _rawInput = input;
        UpdateInput(true);
    }

    public void UpdateInput(object data)
    {
        // project the input according to the camera orientation
        Vector3 camF = _cam.transform.forward;
        Vector3 camR = _cam.transform.right;
        _moveInput = Vector3.ProjectOnPlane(camR * _rawInput.x + camF * _rawInput.y, transform.up).normalized;
    }

    // notify the animator that the player is moving
    public event UnityAction<bool> OnMoving;
    public void SetMoving(bool isMoving) => OnMoving?.Invoke(isMoving);


    //allows other scripts to toggle the move controls
    public bool isControlsOn;


    #endregion

    // _settings
    [SerializeField] MoveSettings _settings;

    [SerializeField] float _offset;
    [SerializeField] float _groundDistance;
    [SerializeField] float _rideHeight;
    [SerializeField] LayerMask _groundMask;


    void Start()
    {
        _cam = Camera.main;

        if (_settings.input != null)
        {
            _settings.input.moveEvent += OnMove;
        }

        isControlsOn = true;
    }


    void Update()
    {

        SetMoving(!XMath.AlmostZero(_moveInput));
        RideOnSurface();
        MoveByInput();
    }


    // this is parenting co
    void RideOnSurface()
    {
        // if we do not find a surface we escape early
        if (!Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _groundDistance, _groundMask)) return;

        // we get the rotator component 
        if (hit.transform.GetComponent<RotateOnClick>() is RotateOnClick rotator)
        {
            if (rotator.isRotating || rotator.isBeingRotated)
            {
                if (transform.parent == null) transform.parent = hit.transform;
                // transform.position = XMath.FILerp(transform.position, hit.point + transform.up * _rideHeight, 8f);
            }
            else
            {
                transform.parent = null;
                transform.position = XMath.FILerp(transform.localPosition, hit.point + transform.up * _rideHeight, 8f);
            }
        }



    }

    float _wallDistance = .4f;

    void MoveByInput()
    {

        // Vector3 NextPosition = transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        Vector3 NextPosition = transform.position + _moveInput * _offset;

        // if after moving in the direction of input we would leave the ground platform
        if (!Physics.Raycast(NextPosition, -transform.up, _groundDistance, _groundMask))
        {
            RecalculateMoveInputAlongEdge();
        }
        // is there is a wall in the way
        else if (Physics.Raycast(transform.position, _moveInput, _wallDistance, _groundMask))
        {
            RecalculateMoveInputAlongWall();
        }
        else
        {
            transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        }
    }

    void RecalculateMoveInputAlongWall()
    {
        // Define ray directions offset by ±45° from the current move direction
        Vector3 clockwiseRayDirection = Quaternion.AngleAxis(45, transform.up) * _moveInput;
        Vector3 counterClockwiseRayDirection = Quaternion.AngleAxis(-45, transform.up) * _moveInput;

        // Perform raycasts from the player's position in both directions
        bool clockwiseHit = Physics.Raycast(transform.position, clockwiseRayDirection, _wallDistance, _groundMask);


        Debug.DrawRay(transform.position, clockwiseRayDirection * _wallDistance, Color.blue);  // Visualize clockwise ray
        Debug.DrawRay(transform.position, counterClockwiseRayDirection * _wallDistance, Color.green); // Visualize counterclockwise ray

        // Decide how to adjust movement based on raycast results
        if (clockwiseHit)
        {
            // Slide along the wall clockwise
            _moveInput = clockwiseRayDirection;
        }
        else
        {
            // Slide along the wall counterclockwise
            _moveInput = counterClockwiseRayDirection;
        }
    }


    void RecalculateMoveInputAlongEdge()
    {

        // Rotate ray start positions by 45 around transfrom up 
        Vector3 clockwiseRayStart = Quaternion.AngleAxis(45, transform.up) * (_moveInput * _offset) + transform.position;
        Vector3 counterClockwiseRayStart = Quaternion.AngleAxis(-45, transform.up) * (_moveInput * _offset) + transform.position;

        // Perform downward raycasts from both rotated positions
        bool clockwiseHit = Physics.Raycast(clockwiseRayStart, -transform.up, out RaycastHit cwHit, _groundDistance, _groundMask);
        bool counterClockwiseHit = Physics.Raycast(counterClockwiseRayStart, -transform.up, out RaycastHit ccwHit, _groundDistance, _groundMask);


        if (clockwiseHit && !counterClockwiseHit)
        {
            _moveInput = (clockwiseRayStart - transform.position).normalized;
        }
        else if (!clockwiseHit && counterClockwiseHit)
        {
            _moveInput = (counterClockwiseRayStart - transform.position).normalized;
        }
        else if (clockwiseHit && counterClockwiseHit)
        {
            // Choose the closer valid hit
            _moveInput = (cwHit.distance < ccwHit.distance ? clockwiseRayStart : counterClockwiseRayStart - transform.position).normalized;
        }
        else
        {
            _moveInput = Vector3.zero;
        }
    }





}
