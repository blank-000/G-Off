
using UnityEngine;
using UnityEngine.InputSystem;

public enum WorldState
{
    Light,
    Dark,
    MidTone
}


public class WorldStateManager : MonoBehaviour
{
    public GameEvent OnStateChange;
    public static WorldStateManager Instance;

    WorldState _state;

    WorldState[] _possibleStates;
    int _stateIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;


        _possibleStates = new WorldState[3]{
            WorldState.Light,
            WorldState.Dark,
            WorldState.MidTone,
        };
        _state = _possibleStates[_stateIndex];

    }

    public WorldState GetCurrentState()
    {
        return _state;
    }

    public Color GetStateColor(WorldState state)
    {
        switch (state)
        {
            case WorldState.Light:
                return Palette.Light;
            case WorldState.MidTone:
                return Palette.MidTone;
            case WorldState.Dark:
                return Palette.Dark;
        }
        return new Color(1f, 1f, 1f, 1f);
    }


    public void NextState(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _stateIndex = (_stateIndex + 1 + _possibleStates.Length) % _possibleStates.Length;
            _state = _possibleStates[_stateIndex];
            OnStateChange.Raise(_state);
        }
    }
    public void PreviousState(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _stateIndex = (_stateIndex - 1 + _possibleStates.Length) % _possibleStates.Length;
            _state = _possibleStates[_stateIndex];
            OnStateChange.Raise(_state);
        }
    }

}
