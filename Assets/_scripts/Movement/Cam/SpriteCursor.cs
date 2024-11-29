using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteCursor : MonoBehaviour
{
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false;

    }



    void Update()
    {

        rectTransform.position = Pointer.current.position.ReadValue();
    }
}
