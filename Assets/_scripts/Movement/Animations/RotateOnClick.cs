using UnityEngine;


// This is a almost one to one repeat of ChangeUpAxis, both of which should be refactored. 
public class RotateOnClick : MonoBehaviour
{
    AudioSource _audioS;
    float RotationStep = 90f;
    Vector3 RotationAxis = Vector3.up;
    public float TimeToComplete;
    public AnimationCurve SmoothFn;

    Quaternion _targetRotation, _startRotation;
    float _timer = 0f;
    bool isRotating;

    public void Start()
    {
        _audioS = GetComponent<AudioSource>();
        _targetRotation = transform.rotation;
    }

    public void HandleStateChange(object data)
    {
        if (data is Vector3 newUp)
        {
            RotationAxis = newUp;
            // Quaternion newTarget = Quaternion.FromToRotation(transform.up, RotationAxis) * transform.rotation;
            // InitializeLerpTo(newTarget.eulerAngles);

        }
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


    public void Activate(int direction)
    {
        if (isRotating) return;
        _audioS.pitch = Random.Range(.95f, 1.05f);
        _audioS.Play();
        Quaternion newTarget = Quaternion.AngleAxis(direction * RotationStep, RotationAxis) * transform.rotation;
        InitializeLerpTo(newTarget);

    }

    // move away from euler angles due to them not working correctly on web build
    void InitializeLerpTo(Quaternion newTarget)
    {
        _targetRotation = newTarget;
        _startRotation = transform.rotation;
        _timer = 0f;
        isRotating = true;
    }


    void CompleteLerp()
    {
        transform.rotation = _targetRotation;
        _timer = 0f;
        isRotating = false;
    }

    void Update()
    {
        if (!isRotating) return;
        // if (Quaternion.Angle(transform.rotation, _targetRotation) < .1f) return;

        _timer += Time.deltaTime;
        float elapsed = _timer / TimeToComplete;

        transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, SmoothFn.Evaluate(elapsed));

        if (_timer > TimeToComplete)
        {
            CompleteLerp();
        }
    }

}
