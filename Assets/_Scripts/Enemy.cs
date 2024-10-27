using System;
using System.IO;
using UnityEngine;

public class Enemy : Actor
{
    #region stats

    public float damage;

    public float score;

    public float speed;

    #endregion

    public override ActorData ActorData => new EnemyData(this);

    protected override void CustomAwake()
    {
    }

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

    protected override void CustomLoad(string path)
    {
    }
}

[Serializable]
public class EnemyData : ActorData
{
    [SerializeField] private float damage;
    [SerializeField] private float score;
    [SerializeField] private float speed;
    [SerializeField] private Color color;

    public EnemyData(Enemy enemy) : base(enemy)
    {
        damage = enemy.damage;
        score = enemy.score;
        speed = enemy.speed;

        color = enemy.GetComponent<SpriteRenderer>().color;
    }
}