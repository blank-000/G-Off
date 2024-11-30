
using UnityEngine;
using UnityEngine.InputSystem;

public class OnClick : MonoBehaviour
{
    InputReader _intput;
    Camera _cam;



    public void Start()
    {
        _cam = Camera.main;
        _intput = WorldStateManager.Instance.Inputs;
        _intput.clickEvent += Click;
        _intput.altClickEvent += AltClick;
    }

    public void AltClick()
    {

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = _cam.ScreenPointToRay(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var rotator = hitInfo.transform.GetComponent<RotateOnClick>();
            if (rotator != null) rotator.Activate(1);
        }
    }
    public void Click()
    {

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = _cam.ScreenPointToRay(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var rotator = hitInfo.transform.GetComponent<RotateOnClick>();
            if (rotator != null) rotator.Activate(-1);
        }
    }
}