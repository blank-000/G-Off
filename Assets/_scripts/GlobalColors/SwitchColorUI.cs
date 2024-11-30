using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchColorUI : MonoBehaviour
{
    public bool Invert;

    RawImage _img;
    TMP_Text _text;

    public Color a, b;


    bool usingText, usingRawImage;
    Color CurrentColor;

    void Start()
    {
        _img = GetComponent<RawImage>();
        _text = GetComponent<TMP_Text>();

        if (_img != null) usingRawImage = true;
        if (_text != null) usingText = true;

        HandleStateChange(WorldStateManager.Instance.State);
    }



    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            switch (state)
            {
                case WorldState.Light:
                    CurrentColor = Invert ? Palette.MidTone : Palette.Dark;
                    break;
                case WorldState.Dark:
                    CurrentColor = Invert ? Palette.MidTone : Palette.Light;
                    break;
            }
        }
        if (usingRawImage) _img.color = CurrentColor;
        if (usingText) _text.color = CurrentColor;
    }
}
