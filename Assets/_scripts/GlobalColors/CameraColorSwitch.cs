using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraColorSwitch : MonoBehaviour
{

    Camera cam;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.State);
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState)
        {
            switch ((WorldState)data)
            {
                case WorldState.Light:
                    cam.backgroundColor = Palette.Light;
                    break;
                case WorldState.Dark:
                    cam.backgroundColor = Palette.Dark;
                    break;
            }
        }
    }

}