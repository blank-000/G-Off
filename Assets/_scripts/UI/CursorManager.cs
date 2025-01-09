using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursorTex, new Vector2(cursorTex.width / 2, cursorTex.height / 2), CursorMode.ForceSoftware);

    }


}
