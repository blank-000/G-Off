using UnityEngine;

public class ExpandToSize : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    public float Damping, Frequency;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        Frequency += Random.Range(-2f, 8f);
        Damping += Random.Range(.01f, .02f);
    }

    void Update()
    {
        // transform.localScale = XMath.FILerp(transform.localScale, Vector3.one * Radius, Speed);
        transform.localScale = XMath.Spring(transform.localScale, Vector3.one, ref velocity, Damping, Frequency, Time.deltaTime);
    }

}
