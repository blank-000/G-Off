using UnityEngine;

public class ChangeUpAxis : MonoBehaviour
{
    public GameEvent OnCameraRotation;
    public InputReader inputs;
    public float TimeToComplete;
    public AnimationCurve SmoothFn;


    Quaternion _targetRotation, _startRotation;
    float _timer = 0f;
    bool isRotating;

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.State);
    }
    void OnEnable()
    {
        inputs.nextEvent += Activate;
    }

    void Activate()
    {
        if (isRotating) return;
        Quaternion newTarget = Quaternion.AngleAxis(-90, transform.up) * transform.rotation;
        InitializeLerpTo(newTarget.eulerAngles);
        OnCameraRotation.Raise(true);

    }

    public void SetLevelRotation(Vector3 levelStartRot)
    {
        InitializeLerpTo(levelStartRot);
    }

    public void HandleStateChange(object data)
    {
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
