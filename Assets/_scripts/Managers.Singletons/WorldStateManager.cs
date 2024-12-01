using UnityEngine;

public enum WorldState
{
    Light,
    Dark
}


public class WorldStateManager : MonoBehaviour
{
    public GameEvent OnStateChange;
    public GameEvent OnAxisChange;
    public GameEvent OnFailedRotationAtempt;

    public InputReader Inputs;

    Transform _player;
    public float MaxCheckDistance = .7f;
    public LayerMask Ground;

    public static WorldStateManager Instance;
    public WorldState State { get; private set; }

    RaycastHit hitInfo;

    void Awake()
    {
        // singleton
        if (Instance == null) Instance = this;

        // input setup
        Inputs.activateEvent += ChangeState;


        State = WorldState.Dark;
        if (_player == null)
        {
            _player = FindFirstObjectByType<Rotate>().transform;
        }
    }


    public bool CanTransitionState()
    {
        return Physics.Raycast(_player.position, _player.forward, out hitInfo, MaxCheckDistance, Ground);
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

    public void ResetState()
    {
        State = WorldState.Dark;
        OnAxisChange.Raise(Vector3.up);
        OnStateChange.Raise(State);
    }

    void ChangeState()
    {
        if (!CanTransitionState())
        {
            // this is very poorly named!!
            OnFailedRotationAtempt.Raise(false);
            return;
        }
        OnFailedRotationAtempt.Raise(true);
        State = (State == WorldState.Dark) ? WorldState.Light : WorldState.Dark;
        OnAxisChange.Raise(hitInfo.normal);
        OnStateChange.Raise(State);
    }

}
