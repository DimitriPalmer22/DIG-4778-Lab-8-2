using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class SaveLoadManager : MonoBehaviour
{
    private const string LOCATION_DATA_FILE = "locationData.json";
    private const string SCORE_DATA_FILE = "scoreData";

    private PlayerController _playerController;

    private string LocationSavePath => $"{Application.persistentDataPath}/{LOCATION_DATA_FILE}";
    private string ScoreSavePath => $"{Application.persistentDataPath}/{SCORE_DATA_FILE}";

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
        Debug.Log($"Saving to {LocationSavePath}");

        // Create a new GameSaver
        var gameSaver = new GameSaver(this);

        // Use the JsonUtility to serialize the data
        var data = JsonUtility.ToJson(gameSaver, true);

        // Save the data to the file
        System.IO.File.WriteAllText(LocationSavePath, data);

        // Create a binary formatter
        var formatter = new BinaryFormatter();
        var stream = new System.IO.FileStream(ScoreSavePath, System.IO.FileMode.Create);

        // Serialize the data
        formatter.Serialize(stream, ScoreManager.Instance.Score);

        // Close the stream
        stream.Close();
    }

    private void OnLoad(InputAction.CallbackContext obj)
    {
        // Open the file
        var data = System.IO.File.ReadAllText(LocationSavePath);

        // Use the JsonUtility to deserialize the data
        var gameSaver = JsonUtility.FromJson<GameSaver>(data);

        // Remove all enemies
        EnemySpawner.Instance.RemoveAllEnemies();

        // Set the player data
        GameManager.Instance.Player.Load(gameSaver.Player);

        // For each enemy data, spawn a new enemy
        foreach (var enemyData in gameSaver.Enemies)
            EnemySpawner.Instance.SpawnEnemyUsingData(enemyData);

        // Load the score
        var formatter = new BinaryFormatter();

        // Open the stream
        var stream = new System.IO.FileStream(ScoreSavePath, System.IO.FileMode.Open);

        // Deserialize the data
        ScoreManager.Instance.Load(formatter, stream);

        // Close the stream
        stream.Close();
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