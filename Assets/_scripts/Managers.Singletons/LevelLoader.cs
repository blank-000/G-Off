using System;
using System.Data;
using Unity.Mathematics;
using UnityEngine;


[System.Serializable]
public struct Level
{
    public string Hint;
    public GameObject LevelObject;
    public bool RequiresStateChange;


    public Level(string hint, GameObject go, bool _requreStateChange = false)
    {
        Hint = hint;
        LevelObject = go;
        RequiresStateChange = _requreStateChange;
    }
}

public class LevelLoader : MonoBehaviour
{

    public bool isDebugging;
    public int DebugStartLevel;

    [Space(40)]
    public InputReader inputs;
    public GameEvent OnLevelLoading;

    public Level[] levels;
    [Space(400)]

    int _currentLevelIndex;
    Transform _player;
    ChangeUpAxis _camRotation;
    Vector3 levelStart = Vector3.zero;
    GameObject currentlevel;





    void Awake()
    {
        inputs.resetLevelEvent += ReloadLevel;
    }

    void Start()
    {
        _player = FindFirstObjectByType<Player>().transform;
        _camRotation = FindFirstObjectByType<ChangeUpAxis>();
    }

    public void StartTheGame(object data)
    {
        if (!isDebugging) LoadLevel(0);
        else
        {
            LoadLevel(DebugStartLevel);
            _currentLevelIndex = DebugStartLevel;
        }
    }


    void LoadLevel(int index)
    {
        if (_player.parent != null) _player.parent = null;
        if (currentlevel != null) Destroy(currentlevel);

        WorldStateManager.Instance.ResetState();
        currentlevel = Instantiate(levels[index].LevelObject, Vector3.zero, Quaternion.identity);

        OnLevelLoading.Raise(levels[index]);

        // string implementation would require three events because of the types
        // OnCurrentLevelHintChanged.Raise(levels[index].Hint);

        _player.position = levelStart;
        // _player.rotation = quaternion.identity;
        _camRotation.SetLevelRotation(Vector3.zero);
    }


    void ReloadLevel()
    {
        LoadLevel(_currentLevelIndex);
    }

    public void LoadNextLevel(object data)
    {
        _currentLevelIndex++;
        LoadLevel(_currentLevelIndex);
    }


}
