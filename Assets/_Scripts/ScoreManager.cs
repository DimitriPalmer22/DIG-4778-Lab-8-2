using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;

    private float _score;

    private void Start()
    {
        Instance = this;
        UpdateText();
    }

    public void AddScore(float amount)
    {
        _score += amount;
    }

    public void UpdateText()
    {
        scoreText.text = $"Score: {_score:0.00}";
    }
}