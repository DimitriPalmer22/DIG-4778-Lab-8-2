using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class SaveLoadManager : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = new PlayerController();
        _playerController.Enable();
    }

    private void Start()
    {
        _playerController.SaveLoad.Save.performed += OnSave;
        _playerController.SaveLoad.Load.performed += OnLoad;
    }

    private void OnSave(InputAction.CallbackContext obj)
    {
        // Get the save path
        var savePath = @"C:/Users/Kesou/Desktop/tmpFolder/gameData.json";

        Debug.Log($"Saving to {savePath}");

        // Create a new GameSaver
        var gameSaver = new GameSaver(this);

        // Use the JsonUtility to serialize the data
        var data = JsonUtility.ToJson(gameSaver, true);

        // Save the data to the file
        System.IO.File.WriteAllText(savePath, data);
    }

    private void OnLoad(InputAction.CallbackContext obj)
    {
    }
}

[Serializable]
public class GameSaver
{
    [SerializeField] private PlayerData player;
    [SerializeField] private EnemyData[] enemies;

    public GameSaver(SaveLoadManager saveLoadManager)
    {
        player = (PlayerData)GameManager.Instance.Player.ActorData;

        enemies = Object
            .FindObjectsOfType<Enemy>()
            .Select(n => (EnemyData)n.ActorData)
            .ToArray();
    }
}