using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileScript : MonoBehaviour, IObjectPooled
{
    [SerializeField] [Min(0)] private float lifetime = 3f;

    [SerializeField] [Min(0)] private float speed = 5;

    [SerializeField] private float degreesPerSecond = 360;

    private float _currentLifetime;

    private int _rotationDirection = 1;

    private Vector3 _direction;

    private void Awake()
    {
        // Randomize the rotation direction
        _rotationDirection = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
    }

    public void Fire(GameObject shooter, Vector2 direction)
    {
        // Set the position to the shooter's position
        transform.position = shooter.transform.position;

        // Set the direction
        _direction = direction;

        // Set the rotation to the shooter's rotation
        transform.up = direction;

        // Set the lifetime
        _currentLifetime = lifetime;
    }

    private void Update()
    {
        // Update the movement
        UpdateMovement();

        // Update the rotation
        UpdateRotation();

        // Update the lifetime
        UpdateLifetime();
    }

    private void UpdateLifetime()
    {
        // Decrease the lifetime
        _currentLifetime -= Time.deltaTime;

        // If the lifetime is less than or equal to 0
        if (_currentLifetime <= 0)
        {
            Debug.Log($"Releasing Bullet!");

            // Return the projectile to the pool
            ProjectilePool.Instance.ReleaseGameObject(gameObject);
        }
    }

    private void UpdateMovement()
    {
        // Move the projectile forward
        transform.position += _direction * (speed * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        // Rotate the projectile
        transform.Rotate(Vector3.forward, degreesPerSecond * _rotationDirection * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"Entering Trigger: {other.name}");

        // Try to get the enemy component
        if (!other.TryGetComponent<Enemy>(out var enemy))
            return;

        enemy.ChangeHealth(-1);

        // Return the projectile to the pool
        ProjectilePool.Instance.ReleaseGameObject(gameObject);
    }

    #region Pooling Functions

    public void OnPoolGet()
    {
    }

    public void OnPoolRelease()
    {
    }

    public void OnPoolDestroy()
    {
    }

    public void OnPoolCreate()
    {
    }

    #endregion
}