using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameEvent OnLevelComplete;
    bool triggered = false;

    RotateOnClick Rotator;

    void Awake()
    {
        Rotator = GetComponentInParent<RotateOnClick>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !triggered && !Rotator.isRotating)
        {
            Debug.Log("next Level");
            OnLevelComplete.Raise(true);
            triggered = true;
        }

    }
}
