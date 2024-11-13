using UnityEngine;

public class LerpSmoothingFollow : MonoBehaviour
{
    public Transform Target;
    [Range(0f, 1f)]
    public float FollowSpeed;

    void Update()
    {
        float r = 1 - FollowSpeed;
        transform.position = (transform.position - Target.position) * Mathf.Pow(r, Time.deltaTime) + Target.position;
    }
}
