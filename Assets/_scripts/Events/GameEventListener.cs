using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public UnityEvent<object> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

}

