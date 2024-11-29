using UnityEngine;

public class Gravity : MonoBehaviour
{
    Vector3 _gravityDirection = Vector3.zero;
    Rigidbody _rb;
    Transform _cam;
    bool hasRotated;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main.transform;
    }

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.State);
    }


    public void HandleStateChange(object data)
    {
        if (data is WorldState state)
        {
            _rb.linearVelocity = Vector3.zero;
            hasRotated = false;
            switch (state)
            {
                case WorldState.Light:
                    // x is up 
                    _gravityDirection = Vector3.forward;
                    break;
                case WorldState.Dark:
                    // y is up. this is the starting state
                    _gravityDirection = Vector3.down;
                    break;
            }
        }
    }



    void FixedUpdate()
    {
        RotateUp();
    }



    void RotateUp()
    {
        if (hasRotated) return;



    }
}
