using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject PlayerGraphic;



    public void StartGame(object data)
    {
        PlayerGraphic.SetActive(true);
    }
}
