using UnityEngine;
using UnityEngine.UI;

public class SwitchColorUI : MonoBehaviour
{
    public bool Invert;
    RawImage _img;

    void Start()
    {
        _img = GetComponent<RawImage>();

        HandleStateChange(WorldStateManager.Instance.State);
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState)
        {
            switch ((WorldState)data)
            {
                case WorldState.Light:
                    _img.color = Invert ? Palette.MidTone : Palette.Dark;
                    break;
                case WorldState.Dark:
                    _img.color = Invert ? Palette.Light : Palette.MidTone;
                    break;
            }
        }
    }
}
