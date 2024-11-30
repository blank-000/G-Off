
using Unity.VisualScripting;
using UnityEngine;

public enum WorldState
{
    Light,
    Dark
}


public class WorldStateManager : MonoBehaviour
{
    public GameEvent OnStateChange;
    public InputReader Inputs;
    public static WorldStateManager Instance;
    public Transform player;
    public Vector3 GravityAxis = Vector3.down;
    public float MaxCheckDistance = .5f;
    public LayerMask Ground;

    public WorldState State { get; private set; }

    WorldState[] _possibleStates;
    int _stateIndex = 0;

    void Awake()
    {
        // singleton
        if (Instance == null) Instance = this;

        // input setup
        Inputs.previousEvent += PreviousState;
        // Inputs.nextEvent += NextState;

        // state setup and init
        _possibleStates = new WorldState[2]{
            WorldState.Dark,
            WorldState.Light
        };
        State = _possibleStates[_stateIndex];
        if (player == null)
        {

            player = FindFirstObjectByType<Rotate>().transform;
        }
    }


    public bool CanTransitionState()
    {
        return Physics.Raycast(player.position, player.forward, MaxCheckDistance, Ground);
    }

    public Color GetStateColor(WorldState state)
    {
        switch (state)
        {
            case WorldState.Light:
                return Palette.Light;
            case WorldState.Dark:
                return Palette.Dark;
        }
        return new Color(1f, 1f, 1f, 1f);
    }


    public void SetState(WorldState state)
    {
        State = state;
        OnStateChange.Raise(State);

    }

    public void NextState()
    {
        if (!CanTransitionState()) return;
        _stateIndex = (_stateIndex + 1 + _possibleStates.Length) % _possibleStates.Length;
        State = _possibleStates[_stateIndex];
        OnStateChange.Raise(State);
    }


    public void PreviousState()
    {
        if (!CanTransitionState()) return;
        _stateIndex = (_stateIndex - 1 + _possibleStates.Length) % _possibleStates.Length;
        State = _possibleStates[_stateIndex];
        OnStateChange.Raise(State);
    }

}
