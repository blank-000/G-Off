using UnityEngine;

public class InitShader : MonoBehaviour
{
    public Material _mat;

    public void RespondToWorldState(object data)
    {
        if (data is WorldState state)
        {
            int isDark = state == WorldState.Dark ? 1 : 0;
            _mat.SetInteger("_IsDark", isDark);
        }
    }

    void Start()
    {
        _mat.SetColor("_Light", Palette.Light);
        _mat.SetColor("_Midtone", Palette.MidTone);
        _mat.SetColor("_Dark", Palette.Dark);
    }


}
