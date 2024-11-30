using TMPro;
using UnityEngine;

public class Hints : MonoBehaviour
{
    TMP_Text _hint;

    void Awake()
    {
        _hint = GetComponent<TMP_Text>();
    }

    public void UpdateHintText(object data)
    {
        if (data is Level level)
        {
            _hint.text = level.Hint;
        }
    }
}
