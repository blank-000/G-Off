
using UnityEngine;
using UnityEngine.InputSystem;

public enum ColorState
{
    Light,
    DarkBlue,
    LightBlue
}


public class WorldState : MonoBehaviour
{
    public GameEvent OnStateChange;
    public static WorldState Instance;

    ColorState _state;

    ColorState[] _states;
    int _stateIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;


        _states = new ColorState[3]{
            ColorState.Light,
            ColorState.DarkBlue,
            ColorState.LightBlue,
        };
        _state = ColorState.Light;

    }

    public ColorState GetState()
    {
        return _state;
    }


    public void NextState(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _stateIndex = (_stateIndex + 1 + _states.Length) % _states.Length;
            _state = _states[_stateIndex];
            OnStateChange.Raise(_state);
        }
    }
    public void PreviousState(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _stateIndex = (_stateIndex - 1 + _states.Length) % _states.Length;
            _state = _states[_stateIndex];
            OnStateChange.Raise(_state);
        }
    }

}
