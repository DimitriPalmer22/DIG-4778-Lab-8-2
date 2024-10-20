using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] [Min(0)] private float bulletsPerSecond;

    private bool _isShooting;

    private float _currentTimeBetweenShots;

    private float TimeBetweenShots => 1 / bulletsPerSecond;

    private bool IsReadyToFire => _currentTimeBetweenShots <= 0;

    private void VerifyObjectPool()
    {
        // If the pool already exists, skip this
        if (ProjectilePool.Instance[bulletPrefab] != null)
            return;

        // Create a projectile pool for the bullet prefab
        // Add the pool to the ProjectilePool
        ProjectilePool.Instance.AddPool(bulletPrefab,
            () => Instantiate(bulletPrefab),
            (obj) =>
            {
                obj.SetActive(true);
                // obj.hideFlags = HideFlags.None;
            },
            (obj) =>
            {
                obj.SetActive(false);
                // obj.hideFlags = HideFlags.HideInHierarchy;
            },
            Destroy,
            true,
            5,
            10
        );
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time between shots
        UpdateTimeBetweenShots();

        if (_isShooting && IsReadyToFire)
            Shoot();
    }

    private void UpdateTimeBetweenShots()
    {
        _currentTimeBetweenShots -= Time.deltaTime;
    }

    private void Shoot()
    {
        // Make sure the object pool is set up
        VerifyObjectPool();

        // Get a projectile from the pool
        var projectile = ProjectilePool.Instance.GetGameObject(bulletPrefab);

        ProjectilePool.Instance.GetScript(projectile).Fire(gameObject, transform.up);

        // Reset the time between shots
        _currentTimeBetweenShots = TimeBetweenShots;
    }

    public void SetShooting(bool isShooting)
    {
        _isShooting = isShooting;
    }
}