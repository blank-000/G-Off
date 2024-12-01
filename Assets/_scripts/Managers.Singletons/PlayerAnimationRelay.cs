using UnityEngine;

public class PlayerAnimationRelay : MonoBehaviour
{
    Animator _animator;
    Move _move;
    public Fire _fire;

    public static PlayerAnimationRelay I;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (I == null) I = this;
        else Destroy(this.gameObject);

        foreach (Transform child in transform)
        {
            if (child.GetComponentInChildren<Animator>() != null && _animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }
        _move = GetComponentInParent<Move>();

    }

    public void RegisterComponent(Component component)
    {
        switch (component)
        {
            case Fire fire:
                _fire = fire;
                _fire.OnFired += OnFireRecieved;
                break;
                // case Move move:
                //     _move = move;
                //     _move.OnMoving += OnMovingChanged;
                //     break;
        }
    }
    // public void DeregisterComponent(Component component)
    // {
    //     switch (component)
    //     {
    //         case Fire fire:
    //             _fire = fire;
    //             _fire.OnFired -= OnFireRecieved;
    //             break;
    //         case Move move:
    //             _move = move;
    //             _move.OnMoving -= OnMovingChanged;
    //             break;
    //     }
    // }

    void OnEnable()
    {
        _move.OnMoving += OnMovingChanged;
    }

    void OnDisable()
    {
        if (_move != null) _move.OnMoving -= OnMovingChanged;
        if (_fire != null) _fire.OnFired -= OnFireRecieved;
    }

    void OnFireRecieved()
    {
        _animator.SetTrigger("Fire");
    }

    void OnMovingChanged(bool isMoving)
    {
        // if (isMoving) Debug.Log("events are on!");
        _animator.SetBool("isMoving", isMoving);
    }
}
