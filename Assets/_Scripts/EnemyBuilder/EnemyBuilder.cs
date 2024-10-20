using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyBuilder
{
    private EnemyBuildInfo _currentEnemyBuildInfo;

    private readonly EnemySpawner _enemySpawner;

    public EnemyBuildInfo CurrentEnemyBuildInfo => _currentEnemyBuildInfo;

    public EnemyBuilder(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
    }


    #region Builder Functions

    public EnemyBuilder ResetEnemyInfo()
    {
        _currentEnemyBuildInfo = new EnemyBuildInfo();
        return this;
    }

    public EnemyBuilder SetEnemySprite(Sprite sprite)
    {
        _currentEnemyBuildInfo.Sprite = sprite;
        return this;
    }

    public EnemyBuilder SetEnemyColor(Color color)
    {
        _currentEnemyBuildInfo.Color = color;
        return this;
    }

    public EnemyBuilder SetEnemyHealth(float health)
    {
        _currentEnemyBuildInfo.Health = health;
        return this;
    }

    public EnemyBuilder SetEnemyDamage(float damage)
    {
        _currentEnemyBuildInfo.Damage = damage;
        return this;
    }

    public EnemyBuilder SetEnemyScore(float score)
    {
        _currentEnemyBuildInfo.Score = score;
        return this;
    }

    public EnemyBuilder SetEnemySpeed(float speed)
    {
        _currentEnemyBuildInfo.Speed = speed;
        return this;
    }

    public EnemyBuilder SetEnemyScale(float scale)
    {
        _currentEnemyBuildInfo.Scale = scale;
        return this;
    }

    public Enemy Result()
    {
        return InstantiateEnemy(_currentEnemyBuildInfo);
    }

    public Enemy InstantiateEnemy(EnemyBuildInfo enemyInfo)
    {
        // Instantiate the enemy prefab
        var enemy = Object.Instantiate(_enemySpawner.baseEnemyPrefab);

        // Get the enemy's script
        var enemyScript = enemy.GetComponent<Enemy>();

        // Set the enemy's sprite renderer
        var spriteRenderer = enemy.GetComponent<SpriteRenderer>();

        // Set the enemy's sprite
        spriteRenderer.sprite = enemyInfo.Sprite;

        // Set the enemy's color
        spriteRenderer.color = enemyInfo.Color;

        // Set the enemy's health
        enemyScript.health = enemyInfo.Health;

        // Set the enemy's damage
        enemyScript.damage = enemyInfo.Damage;

        // Set the enemy's score
        enemyScript.score = enemyInfo.Score;

        // Set the enemy's speed
        enemyScript.speed = enemyInfo.Speed;

        // Set the enemy's scale
        enemy.transform.localScale = Vector3.one * enemyInfo.Scale;

        // Return the enemy
        return enemyScript;
    }

    #endregion
}