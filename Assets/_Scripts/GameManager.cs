using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Player _player;

    #region Getters

    public Player Player => _player;

    #endregion

    private void Awake()
    {
        // Set the instance
        Instance = this;

        // Find the player in the scene
        _player = FindObjectOfType<Player>();
    }
}