using TMPro;
using UnityEngine;

public class OneOff : MonoBehaviour
{
    public InputReader inputs;
    public string textToDisplay;
    TMP_Text textField;

    bool hasSucceded;


    public void CameraIsRotating(object data)
    {
        if (hasSucceded) return;

        if (data is bool)
        {
            if ((bool)data)
            {
                hasSucceded = true;
            }
            else
            {
                DisplayRequest();
            }
        }

    }

    public void DisplayRequest()
    {
        textField.text = textToDisplay;
    }

    void OnEnable()
    {
        textField = GetComponent<TMP_Text>();
    }

}
