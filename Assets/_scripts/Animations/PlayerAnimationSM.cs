using UnityEngine;

public class PlayerAnimationSM : MonoBehaviour
{
    Animator _animator;
    Move _move;
    Fire _fire;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponentInChildren<Animator>() != null && _animator == null || _fire == null)
            {
                _animator = GetComponentInChildren<Animator>();
                _fire = GetComponentInChildren<Fire>();
            }
        }
        _move = GetComponent<Move>();

    }

    void OnEnable()
    {
        _move.OnMoving += OnMovingChanged;
        _fire.OnFired += OnFireRecieved;
    }

    void OnDisable()
    {
        _move.OnMoving -= OnMovingChanged;
        _fire.OnFired -= OnFireRecieved;
    }

    void OnFireRecieved()
    {
        _animator.SetTrigger("Fire");
    }

    void OnMovingChanged(bool isMoving)
    {
        if (isMoving) Debug.Log("events are on!");
        _animator.SetBool("isMoving", isMoving);
    }
}
