using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class SaveLoadManager : MonoBehaviour
{
    private const string LOCATION_DATA_FILE = "locationData.json";
    private const string SCORE_DATA_FILE = "scoreData";

    private PlayerController _playerController;

    private string SavePath => $"C:/Users/Kesou/Desktop/tmpFolder/{LOCATION_DATA_FILE}";

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
        Debug.Log($"Saving to {SavePath}");

        // Create a new GameSaver
        var gameSaver = new GameSaver(this);

        // Use the JsonUtility to serialize the data
        var data = JsonUtility.ToJson(gameSaver, true);

        // Save the data to the file
        System.IO.File.WriteAllText(SavePath, data);
    }

    private void OnLoad(InputAction.CallbackContext obj)
    {
        // Open the file
        var data = System.IO.File.ReadAllText(SavePath);

        // Use the JsonUtility to deserialize the data
        var gameSaver = JsonUtility.FromJson<GameSaver>(data);

        // Remove all enemies
        EnemySpawner.Instance.RemoveAllEnemies();

        // Set the player data
        GameManager.Instance.Player.Load(gameSaver.Player);

        // For each enemy data, spawn a new enemy
        foreach (var enemyData in gameSaver.Enemies)
            EnemySpawner.Instance.SpawnEnemyUsingData(enemyData);

        // Log the data
        Debug.Log($"Player: {gameSaver.Player.Transform.Position} - {gameSaver.Player.CurrentHealth} - {gameSaver.Player.MaxHealth}");
        foreach (var enemy in gameSaver.Enemies)
            Debug.Log($"Enemy: {enemy.Transform.Position} - {enemy.CurrentHealth} - {enemy.MaxHealth}");
    }
}

[Serializable]
public class GameSaver : ISaveDataToken
{
    [SerializeField] private PlayerData player;
    [SerializeField] private EnemyData[] enemies;

    #region Getters

    public PlayerData Player => player;

    public EnemyData[] Enemies => enemies;

    #endregion

    public GameSaver(SaveLoadManager saveLoadManager)
    {
        player = (PlayerData)GameManager.Instance.Player.ActorData;

        enemies = Object
            .FindObjectsOfType<Enemy>()
            .Select(n => (EnemyData)n.ActorData)
            .ToArray();
    }
}