
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    Vector2 _mouseScreenPosition;
    Camera _cam;
    Vector3 WorldPos;
    [HideInInspector]
    public Vector3 ProjectedWorldPos;
    // public Vector3 VerticalOffset;
    public float ReorientationSpeed;
    Vector3 _gravityDirection;
    bool _isReorienting;

    void OnEnable()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        WorldStateManager.Instance.player = this.transform;
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            switch (state)
            {
                case WorldState.Light:
                    // x is up 
                    _gravityDirection = Vector3.forward;
                    WorldStateManager.Instance.GravityAxis = _gravityDirection;
                    StartCoroutine(ReorientUp());
                    break;
                case WorldState.Dark:
                    // y is up. this is the starting state
                    _gravityDirection = Vector3.down;
                    WorldStateManager.Instance.GravityAxis = _gravityDirection;
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
        WorldPos = _cam.ScreenToWorldPoint(new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, (_cam.transform.position - transform.position).magnitude));
        ProjectedWorldPos = Vector3.ProjectOnPlane(WorldPos - transform.position, transform.up);


        Quaternion target = Quaternion.LookRotation(ProjectedWorldPos, transform.up);
        transform.rotation = target;
    }





}
