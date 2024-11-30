using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Gun;
    public GameObject PlayerGraphic;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("GunUnlock"))
        {
            Gun.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    public void StartGame(object data)
    {
        PlayerGraphic.SetActive(true);
    }
}
