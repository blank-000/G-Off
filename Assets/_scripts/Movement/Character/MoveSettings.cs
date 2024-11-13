using UnityEngine;

[CreateAssetMenu(fileName = "Move Settings", menuName = "Settings/Move Settings")]
public class MoveSettings : ScriptableObject
{
    [Header("Input")]
    public InputReader input;
    public bool isRelativeToCamera;

    [Space(20)]
    [Header("Movement Motion Settings")]
    public float maxSpeed;
    public float acceleration;
    public AnimationCurve accelerationFactorFromDot;
    public float maxAccelerationForce;
    public AnimationCurve maxAccelerationFactorFromDot;
    public Vector3 forceScale;

    [Space(20)]
    [Header("Rotation Motion Settings")]
    public float SpringDamper;
    public float SpringStrength;


}