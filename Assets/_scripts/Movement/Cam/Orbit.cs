using UnityEngine;

public class Orbit : MonoBehaviour
{
    public InputReader inputs;

    float RotationStep = 90f;
    float RotationSpeed = 5f;

    Quaternion _targetRotation;
    bool isRotating;

    void Start()
    {
        _targetRotation = transform.rotation;

    }
    void OnEnable()
    {
        inputs.nextEvent += Activate;
    }



    void Activate()
    {
        if (isRotating) return;

        _targetRotation = Quaternion.AngleAxis(RotationStep, transform.up) * transform.rotation;

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
