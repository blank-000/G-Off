using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateOnClick : MonoBehaviour
{
    InputReader _intput;
    Camera _cam;

    public float RotationStep = 90f;
    public Vector3 RotationAxis = Vector3.up;
    public float RotationSpeed = 5f;

    Quaternion targetRotation;

    public void Start()
    {
        targetRotation = transform.rotation;
        _cam = Camera.main;
        _intput = WorldStateManager.Instance.Inputs;
        _intput.attackEvent += OnClick;
    }

    public void OnClick()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = _cam.ScreenPointToRay(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            RotateOnClick rotator = hitInfo.transform.GetComponent<RotateOnClick>();
            if (rotator == null) return;

            rotator.Activate();
        }
    }

    public void Activate()
    {
        float x = RotationStep * RotationAxis.x;
        float y = RotationStep * RotationAxis.y;
        float z = RotationStep * RotationAxis.z;

        targetRotation *= Quaternion.Euler(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
    }
}
