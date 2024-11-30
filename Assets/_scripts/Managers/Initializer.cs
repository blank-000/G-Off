using Unity.Mathematics;
using UnityEngine;


public class Initializer : MonoBehaviour
{
    public GameObject Player;
    public GameObject Camera;
    public GameObject Cursor;
    public GameObject Managers;

    public static Initializer Inst;

    void Awake()
    {
        if (Inst == null) Inst = this;
        if (Managers == null) Managers = this.gameObject;
        Spawn(Player);
        Spawn(Camera);
        Spawn(Cursor);
    }
    void Spawn(GameObject objectToSpawn)
    {
        GameObject obj = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);
        obj.name = objectToSpawn.ToString();
    }

    public Camera GetCamera()
    {
        // Start recursive search from the current transform
        return FindCameraInChildren(Camera.transform);
    }

    private Camera FindCameraInChildren(Transform parent)
    {
        // Check if the current transform has a Camera component
        Camera cam = parent.GetComponent<Camera>();
        if (cam != null)
        {
            return cam; // Return the first Camera found
        }

        // If no Camera, recursively check each child
        foreach (Transform child in parent)
        {
            cam = FindCameraInChildren(child);
            if (cam != null)
            {
                return cam; // Return the Camera if found in any child
            }
        }

        // If no Camera is found in this branch, return null
        return null;
    }


}
