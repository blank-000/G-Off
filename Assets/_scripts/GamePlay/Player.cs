using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Gun;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("GunUnlock"))
        {
            Gun.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}
