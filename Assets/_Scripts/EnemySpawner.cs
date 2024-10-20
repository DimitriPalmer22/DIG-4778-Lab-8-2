using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject baseEnemyPrefab;

    [SerializeField] public Sprite[] enemySprites;

    [SerializeField] public float spawnTimer = 3f;

    private float _currentSpawnTimer;

    private EnemyBuilder _enemyBuilder;

    private void Awake()
    {
        _enemyBuilder = new EnemyBuilder(this);
        _currentSpawnTimer = spawnTimer;
    }

    private void Update()
    {
        // Comment this line out if you want to manually spawn enemies
        AutoSpawn();
    }

    private void AutoSpawn()
    {
        // Update the timer
        UpdateTimer();

        // Return if the timer is not finished
        if (_currentSpawnTimer > 0)
            return;

        const float precisionMod = 10000;

        // Get a random position from -4 to 4 on the x and y axes
        var randomPosition = new Vector2(
            UnityEngine.Random.Range(-4 * precisionMod, 4 * precisionMod) / precisionMod,
            UnityEngine.Random.Range(-4 * precisionMod, 4 * precisionMod) / precisionMod
        );

        // Generate a random number from 1 to 4
        var randomEnemyType = UnityEngine.Random.Range(1, 5);

        var enemy = randomEnemyType switch
        {
            1 => BuildEnemyType1(randomPosition),
            2 => BuildEnemyType2(randomPosition),
            3 => BuildEnemyType3(randomPosition),
            _ => BuildRandomEnemy()
        };

        // Spawn an enemy
        SpawnEnemy(enemy, randomPosition);

        // Reset the timer
        _currentSpawnTimer = spawnTimer;
    }

    private void UpdateTimer()
    {
        _currentSpawnTimer -= Time.deltaTime;
    }

    public EnemyBuildInfo BuildRandomEnemy()
    {
        // Choose a random sprite
        var randomSprite = enemySprites[UnityEngine.Random.Range(0, enemySprites.Length)];

        _enemyBuilder.ResetEnemyInfo().SetEnemySprite(randomSprite).SetEnemyColor(UnityEngine.Random.ColorHSV())
            .SetEnemyHealth(UnityEngine.Random.Range(1, 4)).SetEnemyDamage(UnityEngine.Random.Range(1, 4))
            .SetEnemyScore(_enemyBuilder.CurrentEnemyBuildInfo.Damage + _enemyBuilder.CurrentEnemyBuildInfo.Health)
            .SetEnemySpeed(UnityEngine.Random.Range(1, 4)).SetEnemyScale(UnityEngine.Random.Range(1, 3));

        // Return the enemy info
        return _enemyBuilder.CurrentEnemyBuildInfo;
    }

    public EnemyBuildInfo BuildEnemyType1(Vector2 position)
    {
        _enemyBuilder
            .ResetEnemyInfo()
            .SetEnemySprite(enemySprites[0])
            .SetEnemyColor(Color.magenta)
            .SetEnemyHealth(2)
            .SetEnemyDamage(1)
            .SetEnemyScore(3)
            .SetEnemySpeed(1).SetEnemyScale(1);

        // Return the enemy info
        return _enemyBuilder.CurrentEnemyBuildInfo;
    }

    public EnemyBuildInfo BuildEnemyType2(Vector2 position)
    {
        _enemyBuilder
            .ResetEnemyInfo()
            .SetEnemySprite(enemySprites[0])
            .SetEnemyColor(Color.blue)
            .SetEnemyHealth(1)
            .SetEnemyDamage(2)
            .SetEnemyScore(3)
            .SetEnemySpeed(1).SetEnemyScale(1);

        // Return the enemy info
        return _enemyBuilder.CurrentEnemyBuildInfo;
    }

    public EnemyBuildInfo BuildEnemyType3(Vector2 position)
    {
        _enemyBuilder
            .ResetEnemyInfo()
            .SetEnemySprite(enemySprites[0])
            .SetEnemyColor(Color.red)
            .SetEnemyHealth(1)
            .SetEnemyDamage(1)
            .SetEnemyScore(3)
            .SetEnemySpeed(2).SetEnemyScale(1);

        // Return the enemy info
        return _enemyBuilder.CurrentEnemyBuildInfo;
    }


    public Enemy SpawnEnemy(EnemyBuildInfo enemyInfo, Vector2 position)
    {
        // Build the enemy
        var enemy = _enemyBuilder.InstantiateEnemy(enemyInfo);

        // Set the enemy's position
        enemy.transform.position = position;

        // Set the enemy's rotation
        enemy.transform.rotation = Quaternion.identity;

        return enemy;
    }
}