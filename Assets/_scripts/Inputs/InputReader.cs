using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    // Gameplay events

    public event UnityAction clickEvent;
    public event UnityAction altClickEvent;
    public event UnityAction activateEvent;
    public event UnityAction<Vector2> moveEvent;
    public event UnityAction previousEvent;
    public event UnityAction nextEvent;
    public event UnityAction resetLevelEvent;


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


    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started) previousEvent?.Invoke();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started) nextEvent?.Invoke();
    }


    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started) clickEvent?.Invoke();
    }

    public void OnActivate(InputAction.CallbackContext context)
    {
        if (context.started) activateEvent?.Invoke();
    }

    public void OnAltClick(InputAction.CallbackContext context)
    {
        if (context.started) altClickEvent?.Invoke();
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.started) resetLevelEvent?.Invoke();
    }
}
