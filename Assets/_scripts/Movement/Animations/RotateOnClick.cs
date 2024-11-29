using UnityEngine;

public class RotateOnClick : MonoBehaviour
{
    public float RotationStep = 90f;
    public Vector3 RotationAxis = Vector3.up;
    public float RotationSpeed = 5f;

    Quaternion _targetRotation;
    bool isRotating;

    public void Start()
    {
        _targetRotation = transform.rotation;
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            switch (state)
            {
                case WorldState.Light:
                    // x is up 
                    RotationAxis = Vector3.forward;
                    break;
                case WorldState.Dark:
                    // y is up
                    RotationAxis = Vector3.up;
                    break;
            }
        }
    }


    public void Activate()
    {
        if (isRotating) return;
        // _targetRotation *= Quaternion.Euler(RotationAxis * RotationStep);
        _targetRotation = Quaternion.AngleAxis(RotationStep, RotationAxis) * transform.rotation;

        isRotating = true;
    }


    void Update()
    {
        // reset the rotation values to 0 when the rotation is complete
        if (XMath.PracticallyEqual(_targetRotation, transform.rotation))
        {
            transform.rotation = _targetRotation;
            isRotating = false;
            return;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, RotationSpeed * Time.deltaTime);
    }
}
