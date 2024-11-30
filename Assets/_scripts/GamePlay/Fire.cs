using UnityEngine;
using UnityEngine.Events;


public class Fire : MonoBehaviour
{
    public InputReader inputs;
    public event UnityAction OnFired;

    public GameObject BlobPrefab;
    public Transform _barrel;

    Rotate rotate;

    void Awake()
    {
        rotate = FindFirstObjectByType<Rotate>();
    }
    void OnEnable()
    {
        inputs.clickEvent += OnFireInput;
        PlayerAnimationRelay.I.RegisterComponent(this);
    }
    void OnDisable()
    {
        inputs.clickEvent -= OnFireInput;
    }

    void OnFireInput()
    {
        OnFired?.Invoke();

    }

    public void OnFire()
    {
        // handle animation fire event

        // spawn projectile
        GameObject blob = Instantiate(BlobPrefab, _barrel.position, _barrel.rotation);


        // play sound
        // spawn fx
    }

}
