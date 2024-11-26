using UnityEngine;

public class ExpandToSize : MonoBehaviour
{
    public float Radius;
    public float Speed;
    public AnimationCurve Smoothing;

    void Update()
    {
        transform.localScale = XMath.FILerp(transform.localScale, Vector3.one * Radius, Speed);

    }

}
