using UnityEngine;

public class SpringFollow : MonoBehaviour
{
    public Transform Target;
    public float Frequency, Damping;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 vel = _rb.linearVelocity;
        transform.position = XMath.Spring(transform.position, Target.position, ref vel, Damping, Frequency, Time.deltaTime);
        _rb.linearVelocity = vel;
    }


}
