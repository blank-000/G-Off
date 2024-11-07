using UnityEngine;

public static class Palette
{
    public static readonly Color Light = new Color(251 / 255f, 238 / 255f, 191 / 255f, 255 / 255f);
    public static readonly Color LightBlue = new Color(108 / 255f, 132 / 255f, 149 / 255f, 255 / 255f);
    public static readonly Color DarkBlue = new Color(70 / 255f, 92 / 255f, 115 / 255f, 255 / 255f);
}
[RequireComponent(typeof(Camera))]
public class CameraColorSwitch : MonoBehaviour
{

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        HandleStateChange(WorldState.Instance.GetState());
    }

    public void HandleStateChange(object data)
    {
        if (data is ColorState)
        {
            switch ((ColorState)data)
            {
                case ColorState.Light:
                    Debug.Log("light bg state");
                    cam.backgroundColor = Palette.Light;
                    break;
                case ColorState.LightBlue:
                    Debug.Log("light blue bg state");
                    cam.backgroundColor = Palette.LightBlue;
                    break;
                case ColorState.DarkBlue:
                    Debug.Log("dark blue bg state");
                    cam.backgroundColor = Palette.DarkBlue;
                    break;
            }
        }
    }

}