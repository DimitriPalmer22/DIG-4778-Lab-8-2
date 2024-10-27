using System;
using UnityEngine;

public class Enemy : Actor
{
    #region stats

    public float damage;

    public float score;

    public float speed;

    #endregion


    protected override void CustomStart()
    {
        // Connect the score manager to the event
        OnHit += ScoreChange;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private static void ScoreChange(Actor enemy)
    {
        ScoreManager.Instance.AddScore(((Enemy)enemy).score);
        ScoreManager.Instance.UpdateText();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Try to get the enemy component
        if (!other.gameObject.TryGetComponent<Player>(out var player))
            return;

        // Change the player's health by the enemy's damage
        player.TakeDamage(damage);

        // Destroy the enemy
        Destroy(gameObject);
    }
}