using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : GameObjectPool<ProjectileScript>
{
    public static ProjectilePool Instance { get; private set; }

    protected override void CustomAwake()
    {
        // If the instance already exists, destroy this
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        // Set the instance to this
        Instance = this;
    }

    protected override void CustomUpdate()
    {
    }
}