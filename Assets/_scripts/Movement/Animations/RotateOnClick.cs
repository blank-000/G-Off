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
    RotateOnClick parentRotator;
    public bool isRotating;
    bool hasRotatingParent;
    public bool isBeingRotated
    {
        get
        {
            if (hasRotatingParent) return parentRotator.isRotating;
            else return false;
        }
    }

    void SetupRotationFlags()
    {
        if (transform.parent.CompareTag("Level")) hasRotatingParent = false;
        else
        {
            parentRotator = transform.parent.GetComponent<RotateOnClick>();
            if (parentRotator != null)
                hasRotatingParent = true;
        }
    }

    void OnEnable()
    {
        SetupRotationFlags();
    }

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


    bool checkSnappedRotation()
    {
        float threshold = .5f; // Allow for minor floating-point discrepancies.

        Vector3 euler = transform.eulerAngles;
        return Mathf.Abs(euler.x % RotationStep) < threshold &&
               Mathf.Abs(euler.y % RotationStep) < threshold &&
               Mathf.Abs(euler.z % RotationStep) < threshold;
    }


    void SnapToStep()
    {
        Vector3 euler = transform.eulerAngles;

        euler.x = Mathf.Round(euler.x / RotationStep) * RotationStep;
        euler.y = Mathf.Round(euler.y / RotationStep) * RotationStep;
        euler.z = Mathf.Round(euler.z / RotationStep) * RotationStep;

        transform.eulerAngles = euler;
    }

    void Update()
    {
        // this is some euler crap, that hopefully holds out, but is prone to breaking, if something is wrong with rotation this is likely where it will come from.
        if (!checkSnappedRotation() && !isRotating)
        {
            SnapToStep();
        }
        if (!isRotating) return;


        _timer += Time.deltaTime;
        float elapsed = _timer / TimeToComplete;

        transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, SmoothFn.Evaluate(elapsed));

        if (_timer > TimeToComplete)
        {
            CompleteLerp();
        }
    }

}
