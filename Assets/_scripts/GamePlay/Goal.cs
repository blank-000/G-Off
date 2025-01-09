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

            OnLevelComplete.Raise(true);
            triggered = true;
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player") && !triggered && !Rotator.isRotating)
        {

            OnLevelComplete.Raise(true);
            triggered = true;
        }
    }
}
