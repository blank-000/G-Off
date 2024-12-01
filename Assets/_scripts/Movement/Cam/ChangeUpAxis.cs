using UnityEngine;

public class ChangeUpAxis : MonoBehaviour
{
    public GameEvent OnCameraRotation;
    public InputReader inputs;
    public float TimeToComplete;
    public AnimationCurve SmoothFn;

    AudioSource source;
    Quaternion _targetRotation, _startRotation;
    float _timer = 0f;
    bool isRotating;
    Vector3 _upAxis;

    void Start()
    {
        source = GetComponent<AudioSource>();
        // HandleStateChange(WorldStateManager.Instance.State);
        _upAxis = transform.up;
    }
    void OnEnable()
    {
        inputs.nextEvent += ActivateClockwise;
        inputs.previousEvent += ActivateCounterClockwise;
    }
    void ActivateClockwise()
    {
        if (isRotating) return;
        // rotate around the up axis
        Quaternion newTarget = Quaternion.AngleAxis(-90, _upAxis) * transform.rotation;
        InitializeLerpTo(newTarget.eulerAngles);
        OnCameraRotation.Raise(true);
    }

    void ActivateCounterClockwise()
    {
        if (isRotating) return;
        // rotate around the up axis
        Quaternion newTarget = Quaternion.AngleAxis(90, _upAxis) * transform.rotation;
        InitializeLerpTo(newTarget.eulerAngles);
        OnCameraRotation.Raise(true);

    }

    public void SetLevelRotation(Vector3 levelStartRot)
    {
        InitializeLerpTo(levelStartRot);
        _upAxis = Vector3.up;
    }


    // to remove the floating in the air bug, I have to add 6 degrees of freedom to the gravity change
    // this means I need to dynamically calculate rotations based on the normal of the wall the player is facing, or the player rotation itself. 
    // it sounds like a lot but is actually one of six possible states. 
    // defining it by the wall normal seems most intuitive
    // to implement this I need to add another axis that rotations can be initialized to. 
    // - calculate the rotation here
    // - calculate the rotation on the player
    // - and add an axis to the cubes. 
    // 
    // problem is this below
    // we don't actually care about the state. we just want the updated rotation axis

    // wow this took like 15 minutes, that was cool

    public void HandleStateChange(object data)
    {
        if (data is Vector3 newUp)
        {
            _upAxis = newUp;
            Quaternion newTarget = Quaternion.FromToRotation(transform.up, _upAxis) * transform.rotation;
            InitializeLerpTo(newTarget.eulerAngles);
            OnCameraRotation.Raise(true);
        }

        if (data is WorldState)
        {
            switch ((WorldState)data)
            {
                case WorldState.Light:
                    InitializeLerpTo(new Vector3(0f, 90f, -90f));
                    break;
                case WorldState.Dark:
                    InitializeLerpTo(Vector3.zero);
                    break;

            }
        }
    }

    void InitializeLerpTo(Vector3 newTarget)
    {
        _targetRotation = Quaternion.Euler(newTarget);
        _startRotation = transform.rotation;
        _timer = 0f;
        isRotating = true;
        source.pitch = Random.Range(.9f, 1.2f);
        source.Play();
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

        _timer += Time.deltaTime;
        float elapsed = _timer / TimeToComplete;

        transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, SmoothFn.Evaluate(elapsed));

        if (_timer > TimeToComplete)
        {
            CompleteLerp();
        }
    }

}
