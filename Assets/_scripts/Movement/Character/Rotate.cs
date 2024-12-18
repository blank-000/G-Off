
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    Vector2 _mouseScreenPosition;
    Camera _cam;
    Vector3 _worldLookAtPosition;
    [HideInInspector]
    public Vector3 ProjectedWorldPos;
    public float ReorientationSpeed;

    Vector3 _gravityDirection;
    bool _isReorienting;



    void Start()
    {
        _cam = Camera.main;
    }

    public void HandleStateChange(object data)
    {

        if (data is Vector3 newUp)
        {
            // Avoid redundant reorientation if the direction is nearly the same
            if (Vector3.Dot(_gravityDirection, -newUp) > 0.99f) return;
            _gravityDirection = -newUp;
            StartCoroutine(ReorientUp());
        }
        if (data is WorldState state)
        {
            switch (state)
            {
                case WorldState.Light:
                    // x is up 
                    _gravityDirection = Vector3.forward;
                    StartCoroutine(ReorientUp());
                    break;
                case WorldState.Dark:
                    // y is up. this is the starting state
                    _gravityDirection = Vector3.down;
                    StartCoroutine(ReorientUp());
                    break;
            }
        }
    }



    IEnumerator ReorientUp()
    {
        _isReorienting = true;

        // Define the target rotation based on -_gravityDirection.
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -_gravityDirection) * transform.rotation;

        // Smoothly interpolate rotation.
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * ReorientationSpeed);
            yield return null;
        }

        // Snap to the exact target rotation to avoid precision issues.
        transform.rotation = targetRotation;
        _isReorienting = false;
    }

    void FixedUpdate()
    {
        // do not interupt the coroutine
        if (_isReorienting) return;


        _mouseScreenPosition = Mouse.current.position.ReadValue();
        _worldLookAtPosition = _cam.ScreenToWorldPoint(new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, (_cam.transform.position - transform.position).magnitude));
        ProjectedWorldPos = Vector3.ProjectOnPlane(_worldLookAtPosition - transform.position, transform.up);


        Quaternion target = Quaternion.LookRotation(ProjectedWorldPos, transform.up);
        transform.rotation = target;
    }
}
