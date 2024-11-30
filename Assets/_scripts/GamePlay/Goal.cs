using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameEvent OnLevelComplete;
    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !triggered)
        {
            Debug.Log("next Level");
            OnLevelComplete.Raise(true);
            triggered = true;
        }

    }
}
