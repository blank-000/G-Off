using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Platform : MonoBehaviour
{
    public WorldState DisabledState;

    Collider col;
    MeshRenderer rend;
    Material mat;

    void Awake()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
        mat = rend.sharedMaterial;
    }

    void Start()
    {
        HandleStateChange(WorldStateManager.Instance.GetCurrentState());
        mat.color = WorldStateManager.Instance.GetStateColor(DisabledState);
    }

    public void HandleStateChange(object data)
    {
        if (data is WorldState && (WorldState)data == DisabledState)
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
