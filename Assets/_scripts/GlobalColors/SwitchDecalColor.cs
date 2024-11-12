using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwitchDecalColor : MonoBehaviour
{
    Material _mat;

    void Start()
    {
        _mat = GetComponent<DecalProjector>().material;

        HandleStateChange(WorldStateManager.Instance.State);
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState)
        {
            switch ((WorldState)data)
            {
                case WorldState.Light:
                    _mat.SetColor("_ShadowColor", Palette.MidTone);
                    break;
                case WorldState.Dark:
                    _mat.SetColor("_ShadowColor", Palette.Light);
                    break;
            }
        }
    }
}
