using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverText;

    private Player _player;

    private float _score;

    private void Awake()
    {
        // Find the player object
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        Instance = this;

        // Disable the game over text
        SetGameOverText(false);

        // Update the text
        UpdateText();
    }

    public void AddScore(float amount)
    {
        _score += amount;
    }

    public void UpdateText()
    {
        scoreText.text = $"Health: {_player.CurrentHealth:0}\n" +
                         $"Score: {_score:0}";
    }

    public void SetGameOverText(bool active)
    {
        gameOverText.gameObject.SetActive(active);
    }
}