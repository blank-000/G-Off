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
    float _wallDistance = .4f;


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
        UpdateInput(true);
        Debug.Log($"Move Input: {_moveInput}");
        RideOnSurface();
        MoveByInput();
        SetMoving(!XMath.AlmostZero(_moveInput));
    }


    void RideOnSurface()
    {
        // if we do not find a surface we escape early
        if (!Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _groundDistance, _groundMask)) return;

        // get the rotator component 
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



    void MoveByInput()
    {

        // added offset instead of (_settings.maxSpeed * Time.deltaTime;) 
        // to better control how far from the edge is acceptable to travel
        Vector3 NextPosition = transform.position + _moveInput * _offset;

        // if after moving in the direction of input we would leave the platform
        if (!Physics.Raycast(NextPosition, -transform.up, _groundDistance, _groundMask))
        {
            RecalculateMoveInputAlongEdge();
            transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        }
        // is there is a wall in the way
        else if (Physics.Raycast(transform.position, _moveInput, _wallDistance, _groundMask))
        {
            RecalculateMoveInputAlongWall();
            transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        }
    }


    void RecalculateMoveInputAlongWall()
    {
        // Define ray directions offset by ±45° from the current move direction
        Vector3 rightDirection = Quaternion.AngleAxis(45, transform.up) * _moveInput;
        Vector3 leftDirection = Quaternion.AngleAxis(-45, transform.up) * _moveInput;

        bool rightHit = Physics.Raycast(transform.position, rightDirection, _wallDistance, _groundMask);
        bool leftHit = Physics.Raycast(transform.position, leftDirection - transform.up, _wallDistance, _groundMask);

        // debugging
        // Color left = leftHit ? Color.green : Color.red;
        // Color right = rightHit ? Color.green : Color.red;
        // Debug.DrawLine(transform.position, transform.position + rightDirection * _wallDistance, right);   // Visualize right ray
        // Debug.DrawLine(transform.position, transform.position + leftDirection * _wallDistance, left); // Visualize left ray

        // Determine valid sliding direction
        if (!rightHit && leftHit)
        {
            _moveInput = rightDirection.normalized;
        }
        else if (!leftHit && rightHit)
        {
            _moveInput = leftDirection.normalized;
        }
        else
        {
            // Stop at corners or if both sides are invalid
            _moveInput = Vector3.zero;
        }
    }


    void RecalculateMoveInputAlongEdge()
    {
        Vector3 rightDirection = Quaternion.AngleAxis(45, transform.up) * _moveInput;
        Vector3 leftDirection = Quaternion.AngleAxis(-45, transform.up) * _moveInput;

        Vector3 rightRayOrigin = transform.position + rightDirection * _offset;
        Vector3 leftRayOrigin = transform.position + leftDirection * _offset;

        // Perform raycasts to the right and left
        bool rightHit = Physics.Raycast(rightRayOrigin, -transform.up, _groundDistance, _groundMask);
        bool leftHit = Physics.Raycast(leftRayOrigin, -transform.up, _groundDistance, _groundMask);

        // debugging
        // Color left = leftHit ? Color.green : Color.red;
        // Color right = rightHit ? Color.green : Color.red;
        // Debug.DrawLine(rightRayOrigin, rightRayOrigin + -transform.up * _groundDistance, right);   // Visualize right ray
        // Debug.DrawLine(leftRayOrigin, leftRayOrigin + -transform.up * _groundDistance, left); // Visualize left ray

        // Determine valid sliding direction
        if (rightHit && !leftHit)
        {
            _moveInput = rightDirection.normalized;
        }
        else if (leftHit && !rightHit)
        {
            _moveInput = leftDirection.normalized;
        }
        else
        {
            // Stop at corners or if both sides are invalid
            _moveInput = Vector3.zero;
        }
    }

}
