using UnityEngine;
using UnityEngine.Events;


public class Fire : MonoBehaviour
{
    public InputReader inputs;
    public event UnityAction OnFired;

    public GameObject BlobPrefab;
    public Transform _barrel;

    void OnEnable()
    {
        inputs.jumpEvent += OnFireInput;
    }
    void OnDisable()
    {
        inputs.jumpEvent -= OnFireInput;
    }

    void OnFireInput()
    {
        OnFired?.Invoke();

    }

    public void OnFire()
    {
        // handle animation fire event

        // spawn projectile
        GameObject blob = Instantiate(BlobPrefab);
        blob.transform.position = _barrel.position;
        blob.transform.rotation = _barrel.rotation;
        // play sound
        // spawn fx
    }

}
