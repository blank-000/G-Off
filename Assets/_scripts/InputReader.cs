using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    // Gameplay events
    public event UnityAction jumpEvent;
    public event UnityAction jumpCanceledEvent;
    public event UnityAction attackEvent;
    public event UnityAction interactEvent;
    public event UnityAction<Vector2> moveEvent;
    public event UnityAction<Vector2> lookEvent;
    public event UnityAction<bool> crouchEvent;
    public event UnityAction<bool> sprintEvent;
    public event UnityAction previousEvent;
    public event UnityAction nextEvent;


    private Controls Controls;

    private void OnEnable()
    {
        if (Controls == null)
        {
            Controls = new Controls();
            Controls.Player.SetCallbacks(this);
        }

        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) attackEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started) interactEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        crouchEvent?.Invoke(context.ReadValueAsButton());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) jumpEvent?.Invoke();
        if (context.canceled) jumpCanceledEvent?.Invoke();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started) previousEvent?.Invoke();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started) nextEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprintEvent?.Invoke(context.ReadValueAsButton());
    }


}
