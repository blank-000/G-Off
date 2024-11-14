using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizer : MonoBehaviour
{

    TextMeshProUGUI _textField;

    public LocalizedString localizedString;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        _textField.text = localizedString.value;
    }


}
