using UnityEngine;

public class ChangeUpAxis : MonoBehaviour
{
    public float TimeToComplete;
    public AnimationCurve SmoothFn;

    Quaternion _targetRotation, _startRotation;
    float _timer = 0f;

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.State);
    }


    public void HandleStateChange(object data)
    {
        if (data is WorldState)
        {
            switch ((WorldState)data)
            {
                case WorldState.Light:
                    InitializeLerpTo(new Vector3(-90f, 0f, 0f));
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
    }

    void CompleteLerp()
    {
        transform.rotation = _targetRotation;
        _timer = 0f;
    }

    void Update()
    {
        if (Quaternion.Angle(transform.rotation, _targetRotation) < .1f) return;

        _timer += Time.deltaTime;
        float elapsed = _timer / TimeToComplete;

        transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, SmoothFn.Evaluate(elapsed));

        if (_timer > TimeToComplete)
        {
            CompleteLerp();
        }
    }


}
