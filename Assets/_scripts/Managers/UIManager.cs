using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Texture2D lightCursor, darkCursor;

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.State);
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            switch (state)
            {
                case WorldState.Light:
                    Cursor.SetCursor(lightCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case WorldState.Dark:
                    Cursor.SetCursor(darkCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }
}
