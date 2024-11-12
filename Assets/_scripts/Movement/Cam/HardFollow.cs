using UnityEngine;

public class HardFollow : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _trackingAxis = Vector3.one;

    void Update()
    {
        Vector3 targetPosition = new Vector3(
            _trackingAxis.x * _target.position.x,
            _trackingAxis.y * _target.position.y,
            _trackingAxis.z * _target.position.z);

        transform.position = targetPosition;
    }
}
