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
        _audioS.Play();
        Quaternion newTarget = Quaternion.AngleAxis(RotationStep, RotationAxis) * transform.rotation;
        InitializeLerpTo(newTarget.eulerAngles);

    }


    void InitializeLerpTo(Vector3 newTarget)
    {
        Vector3 eulerSigns = new Vector3(
                Mathf.Sign(newTarget.x),
                Mathf.Sign(newTarget.y),
                Mathf.Sign(newTarget.z)
            );
        Vector3 eulerAbs = new Vector3(
                Mathf.Abs(newTarget.x),
                Mathf.Abs(newTarget.y),
                Mathf.Abs(newTarget.z)
            );

        Vector3 snappedEuler = new Vector3(
            eulerSigns.x * (eulerAbs.x - (eulerAbs.x % RotationStep)),
            eulerSigns.y * (eulerAbs.y - (eulerAbs.y % RotationStep)),
            eulerSigns.z * (eulerAbs.z - (eulerAbs.z % RotationStep))
        );
        _targetRotation = Quaternion.Euler(snappedEuler);
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
