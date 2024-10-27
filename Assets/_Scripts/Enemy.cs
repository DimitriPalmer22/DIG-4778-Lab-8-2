using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region stats

    public float health;

    public float damage;

    public float score;

    public float speed;

    #endregion

    public event Action<Enemy> OnHit;

    private void Start()
    {
        // Connect the score manager to the event
        OnHit += ScoreChange;

        // Connect the destroy function to the event
        OnHit += DestroyWhenHealthIsZero;
    }

    private void OnDestroy()
    {
    }


    public void ChangeHealth(float amount)
    {
        health += amount;

        // If the enemy is taking damage, call the event
        if (amount < 0)
            OnHit?.Invoke(this);
    }

    private static void ScoreChange(Enemy enemy)
    {
        ScoreManager.Instance.AddScore(enemy.score);
        ScoreManager.Instance.UpdateText();
    }

    private static void DestroyWhenHealthIsZero(Enemy enemy)
    {
        if (enemy.health <= 0)
            Destroy(enemy.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collision with {other.gameObject.name}");

        // Try to get the enemy component
        if (!other.gameObject.TryGetComponent<Player>(out var player))
            return;

        // Change the player's health by the enemy's damage
        player.ChangeHealth(-damage);

        // Destroy the enemy
        Destroy(gameObject);
    }
}