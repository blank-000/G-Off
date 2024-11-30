using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Level
{
    public GameObject levelGeometry;
    public Vector3 CameraStartRotation;

    public Level(GameObject geo, Vector3 camAngle)
    {
        levelGeometry = geo;
        CameraStartRotation = camAngle;
    }
}

public class LevelLoader : MonoBehaviour
{
    int nextLvl, currentLvl;
    public Level[] levels;
    Transform _player;
    ChangeUpAxis _camRotation;
    Vector3 levelStart = Vector3.zero;

    void Awake()
    {
        levels = new Level[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            Level level = new Level(child.gameObject, Vector3.zero);
            levels[i] = level;
            i++;
        }
    }

    void Start()
    {
        _player = FindFirstObjectByType<Player>().transform;
        _camRotation = FindFirstObjectByType<ChangeUpAxis>();
        currentLvl = 0;
        nextLvl = currentLvl + 1;
        _player.position = levelStart;
        _camRotation.SetLevelRotation(levels[nextLvl].CameraStartRotation);
    }



    public void LoadNextScene(object data)
    {
        WorldStateManager.Instance.SetState(WorldState.Dark);
        levels[currentLvl].levelGeometry.gameObject.SetActive(false);
        if (nextLvl < levels.Length)
        {
            _player.position = levelStart;
            _player.rotation = quaternion.identity;
            _camRotation.SetLevelRotation(levels[nextLvl].CameraStartRotation);
            levels[nextLvl].levelGeometry.gameObject.SetActive(true);
        }
        currentLvl = nextLvl;
        nextLvl = currentLvl + 1;

    }
}
