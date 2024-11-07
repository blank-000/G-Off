using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Platform : MonoBehaviour
{
    public ColorState DisabledState;

    Collider col;
    MeshRenderer rend;

    void Awake()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        HandleStateChange(WorldState.Instance.GetState());
    }

    public void HandleStateChange(object data)
    {
        if (data is ColorState && (ColorState)data == DisabledState)
        {
            col.enabled = false;
            rend.enabled = false;
        }
        else
        {
            col.enabled = true;
            rend.enabled = true;
        }
    }
}
