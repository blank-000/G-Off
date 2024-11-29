using Unity.VisualScripting;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public GameObject prefab;
    void OnTriggerEnter(Collider other)
    {
        GameObject splash = Instantiate(prefab, transform.position, transform.rotation);

        splash.transform.SetParent(other.transform);
        Destroy(this.gameObject);
    }
}
