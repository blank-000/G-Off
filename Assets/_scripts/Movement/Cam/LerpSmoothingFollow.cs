using Unity.VisualScripting;
using UnityEngine;

public class LerpSmoothingFollow : MonoBehaviour
{
    public Transform Target;
    public float DecaySlope;

    void OnEnable()
    {
        if (Target == null)
        {
            Target = FindFirstObjectByType<Move>().transform;
        }
    }

    void Update()
    {
        transform.position = XMath.FILerp(transform.position, Target.position, DecaySlope);
    }

}
