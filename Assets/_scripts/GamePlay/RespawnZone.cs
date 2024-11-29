using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnZone : MonoBehaviour
{
    public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawnPoint.transform.position;
    }
}
