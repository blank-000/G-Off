using UnityEngine;

public class End : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = FindFirstObjectByType<Player>().gameObject;
        player.SetActive(false);
    }
}
