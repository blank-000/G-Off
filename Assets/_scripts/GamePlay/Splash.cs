using Unity.VisualScripting;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public GameObject prefab;
    void OnTriggerEnter()
    {
        GameObject splash = Instantiate(prefab);
        prefab.transform.position = transform.position;
        prefab.transform.rotation = transform.rotation;
        Destroy(this.gameObject);
    }
}
