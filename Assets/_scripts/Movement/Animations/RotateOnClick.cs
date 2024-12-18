using System;
using UnityEngine;
using UnityEngine.Events;


// This is a almost one to one repeat of ChangeUpAxis, both of which should be refactored. 
public class RotateOnClick : MonoBehaviour
{
    public bool IsGoal;
    AudioSource _audioS;
    float RotationStep = 90f;
    Vector3 RotationAxis = Vector3.up;
    public float TimeToComplete;
    public AnimationCurve SmoothFn;
    [HideInInspector]
    public event UnityAction<int> rotationEvent;

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
    public ExpandToSize _sizerNormal, _sizerHidden;


    void SetupRotationFlags()
    {
        if (transform.parent.CompareTag("Level")) hasRotatingParent = false;
        else
        {
            parentRotator = transform.parent.GetComponent<RotateOnClick>();
            if (parentRotator != null)
            {
                hasRotatingParent = true;
                parentRotator.rotationEvent += PassOnRotation;
            }
        }
    }


    void OnEnable()
    {
        SetupRotationFlags();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || IsGoal) return;
        _sizerNormal.SetTarget(Vector3.zero);
        _sizerHidden.SetTarget(Vector3.one);
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || IsGoal) return;
        _sizerNormal.SetTarget(Vector3.one);
        _sizerHidden.SetTarget(Vector3.zero);
    }


    public void Start()
    {

        _audioS = GetComponent<AudioSource>();
        _targetRotation = transform.rotation;
        transform.localPosition = new Vector3(
            Mathf.Round(transform.localPosition.x),
            Mathf.Round(transform.localPosition.y),
            Mathf.Round(transform.localPosition.z)

        );
    }

    public void HandleStateChange(object data)
    {
        if (data is Vector3 newUp)
        {
            RotationAxis = newUp;

        }
    }

    void PassOnRotation(int dir)
    {
        Activate(dir);
    }

    public void Activate(int direction)
    {
        if (isRotating) return;
        if (rotationEvent != null) rotationEvent.Invoke(direction);
        _audioS.pitch = UnityEngine.Random.Range(.95f, 1.05f);
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
