using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameEvent StartGame;

    bool hasTriggered;

    public void StartTheGame()
    {
        if (hasTriggered) return;
        StartGame.Raise(true);
        hasTriggered = true;
    }
}
