using System;
using System.IO;
using System.Linq;
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

    protected override void CustomLoad(ActorData data)
    {
        var enemyData = (EnemyData)data;

        damage = enemyData.Damage;
        score = enemyData.Score;
        speed = enemyData.Speed;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = enemyData.Color;

        var spriteIndex = enemyData.SpriteIndex;
        spriteRenderer.sprite =
            EnemySpawner.Instance.EnemySprites.ToArray()[spriteIndex];

        GetComponent<EnemyMovement>().speed = enemyData.MoveDirection;
    }
}

[Serializable]
public class EnemyData : ActorData
{
    [SerializeField] private float damage;
    [SerializeField] private float score;
    [SerializeField] private float speed;
    [SerializeField] private Color color;
    [SerializeField] private int spriteIndex;
    [SerializeField] private float moveDirection;

    #region Getters

    public float Damage => damage;
    public float Score => score;
    public float Speed => speed;
    public Color Color => color;
    public int SpriteIndex => spriteIndex;
    public float MoveDirection => moveDirection;

    #endregion

    public EnemyData(Enemy enemy) : base(enemy)
    {
        damage = enemy.damage;
        score = enemy.score;
        speed = enemy.speed;

        var spriteRenderer = enemy.GetComponent<SpriteRenderer>();

        color = spriteRenderer.color;
        spriteIndex = EnemySpawner.Instance.GetSpriteIndex(spriteRenderer.sprite);

        moveDirection = enemy.GetComponent<EnemyMovement>().speed;
    }
}