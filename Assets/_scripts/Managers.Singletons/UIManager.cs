using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Texture2D lightCursor, darkCursor;
    Vector2 cursorHotspot = new Vector2(32, 32);

    void Start()
    {
        cursorHotspot = new Vector2(lightCursor.width / 2, lightCursor.height / 2f);
        HandleStateChange(WorldStateManager.Instance.State);
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            switch (state)
            {
                case WorldState.Light:
                    Cursor.SetCursor(lightCursor, cursorHotspot, CursorMode.Auto);
                    break;
                case WorldState.Dark:
                    Cursor.SetCursor(darkCursor, cursorHotspot, CursorMode.Auto);
                    break;
            }
        }
    }
}
