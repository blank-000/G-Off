using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColorSwitch : MonoBehaviour
{

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.GetCurrentState());
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState)
        {
            switch ((WorldState)data)
            {
                case WorldState.Light:
                    Debug.Log("light bg state");
                    cam.backgroundColor = Palette.Light;
                    break;
                case WorldState.MidTone:
                    Debug.Log("light blue bg state");
                    cam.backgroundColor = Palette.MidTone;
                    break;
                case WorldState.Dark:
                    Debug.Log("dark blue bg state");
                    cam.backgroundColor = Palette.Dark;
                    break;
            }
        }
    }

}