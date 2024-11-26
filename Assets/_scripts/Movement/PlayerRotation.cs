using UnityEngine;
using UnityEngine.Playables;

public class PlayerRotation : MonoBehaviour
{
    public float RotationSpeed = 5f;
    private Rigidbody _rb;
    [SerializeField] MoveSettings _settings;

    private Vector3 _currentDirection = Vector3.forward;
    private bool _hasInput = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_settings.input != null)
        {
            _settings.input.moveEvent += OnMove;
        }
    }

    public void OnMove(Vector2 input)
    {
        // Convert input into world-space direction
        Vector3 newInputDirection = new Vector3(input.x, 0, input.y);
        _currentDirection = newInputDirection.normalized;

    }

    void FixedUpdate()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {

    }
}
