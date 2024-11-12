using UnityEngine;

public class InitShader : MonoBehaviour
{
    public Material mat;


    void Start()
    {
        mat.SetColor("_Light", Palette.Light);
        mat.SetColor("_Midtone", Palette.MidTone);
        mat.SetColor("_Dark", Palette.Dark);
    }


}
