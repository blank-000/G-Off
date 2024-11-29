using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
    #region INPUT && Events

    Camera _cam;
    Vector3 _moveInput;


    // recieve input and align it to the camera view in relation to the player up 
    public void OnMove(Vector2 input)
    {
        if (!isControlsOn) return;

        // project the input according to the camera orientation
        Vector3 camF = _cam.transform.forward;
        Vector3 camR = _cam.transform.right;
        _moveInput = Vector3.ProjectOnPlane(camR * input.x + camF * input.y, transform.up).normalized;

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


    void Awake()
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
        // RideOnSurface();
        MoveByInput();
    }


    void RideOnSurface()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _groundDistance, _groundMask))
        {
            Vector3 VerticalPosition = transform.up;
            transform.position = XMath.FILerp(transform.localPosition, hit.point + transform.up * _rideHeight, 8f);
        }

    }

    RaycastHit hitInfo;
    void MoveByInput()
    {
        Vector3 NextPosition = transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        // if after moving in the direction of input we would leave the ground platform
        if (!Physics.Raycast(NextPosition, -transform.up, out hitInfo, _groundDistance, _groundMask))
        {
            RecalculateMoveInputAlongEdge(hitInfo);
        }
        else
        {
            transform.position += _moveInput * _settings.maxSpeed * Time.deltaTime;
        }
    }


    void RecalculateMoveInputAlongEdge(RaycastHit hit)
    {
        // calculate a ray _offset in the moveinput direction;
        Vector3 rayStart = transform.position + (_moveInput * _offset);
        _moveInput = Vector3.ProjectOnPlane(_moveInput, hitInfo.normal);
        Debug.DrawLine(rayStart, rayStart - transform.up, Color.red);
    }



}
