
using UnityEngine;

public enum WorldState
{
    Light,
    Dark,
    MidTone
}


public class WorldStateManager : MonoBehaviour
{
    public GameEvent OnStateChange;
    public InputReader Inputs;
    public static WorldStateManager Instance;

    public WorldState State { get; private set; }

    WorldState[] _possibleStates;
    int _stateIndex = 0;

    void Awake()
    {
        // singleton
        if (Instance == null) Instance = this;

        // input setup
        Inputs.previousEvent += PreviousState;
        Inputs.nextEvent += NextState;

        // state setup and init
        _possibleStates = new WorldState[3]{
            WorldState.Light,
            WorldState.Dark,
            WorldState.MidTone,
        };
        State = _possibleStates[_stateIndex];

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


    public void NextState()
    {
        _stateIndex = (_stateIndex + 1 + _possibleStates.Length) % _possibleStates.Length;
        State = _possibleStates[_stateIndex];
        OnStateChange.Raise(State);
    }
    public void PreviousState()
    {
        _stateIndex = (_stateIndex - 1 + _possibleStates.Length) % _possibleStates.Length;
        State = _possibleStates[_stateIndex];
        OnStateChange.Raise(State);
    }

}
