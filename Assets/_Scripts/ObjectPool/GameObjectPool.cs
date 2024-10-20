using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GameObjectPool<TObjectType> : MonoBehaviour
    where TObjectType : MonoBehaviour, IObjectPooled
{
    // Indexer for the pools
    public ObjectPool<GameObject> this[GameObject prefab] => _pools.GetValueOrDefault(prefab);

    // Dictionary of prefabs and their corresponding pools
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _pools = new();

    // A dictionary of object instances and their corresponding scripts
    private readonly Dictionary<GameObject, TObjectType> _scripts = new();

    /// <summary>
    /// A list of all the pool data.
    /// This is used to interact with the pool's data in the inspector.
    /// </summary>
    [SerializeField, Header("Inspector Data. DO NOT TOUCH. For debugging purposes only.")]
    private List<ObjectPoolEntryData> poolData = new();

    /// <summary>
    /// A list of the active game objects and their corresponding prefabs
    /// </summary>
    private readonly Dictionary<GameObject, GameObject> _activeGameObjects = new();

    private void Awake()
    {
        CustomAwake();
    }

    protected abstract void CustomAwake();

    private void Start()
    {
    }

    private void Update()
    {
        // Update the pool data
        UpdatePoolData();

        // Custom update
        CustomUpdate();
    }


    private void UpdatePoolData()
    {
        // For each pool
        foreach (var poolKey in _pools.Keys)
        {
            // Get the pool
            var pool = _pools[poolKey];


            // Get the pool data
            var data = GetPoolData(poolKey);

            data.CountAll = pool.CountAll;
            data.CountActive = pool.CountActive;
            data.CountInactive = pool.CountInactive;
        }
    }

    protected abstract void CustomUpdate();


    private void OnDestroy()
    {
        // Destroy all the pools
        foreach (var pool in _pools)
            pool.Value.Clear();

        // Clear the pools
        _pools.Clear();

        // Clear the scripts
        _scripts.Clear();

        // Clear the pool data
        poolData.Clear();
    }


    private ObjectPoolEntryData GetPoolData(GameObject prefab)
    {
        // Return null if the pool doesn't exist
        if (!_pools.TryGetValue(prefab, out var pool))
            return null;

        // Return the pool data
        return poolData.Find(data => data.Prefab == prefab);
    }

    #region Public Methods

    public void AddPool(GameObject prefab,
        Func<GameObject> createFunc,
        Action<GameObject> onGetFunc,
        Action<GameObject> onReleaseFunc,
        Action<GameObject> onDestroyFunc,
        bool collectionCheck = true,
        int defaultCapacity = 10,
        int maxCapacity = 10000
    )
    {
        // If the pool already exists, skip this
        if (_pools.TryGetValue(prefab, out _))
            return;

        // Create a new pool for the prefab
        var pool = new ObjectPool<GameObject>(
            CreateAndConnectScript,
            onGetFunc + CallGetFunc,
            onReleaseFunc + CallReleaseFunc,
            RemoveScriptOnDestroy + onDestroyFunc,
            collectionCheck,
            defaultCapacity,
            maxCapacity
        );

        // Log
        Debug.Log(
            $"Added pool for {prefab.name} with initial capacity of {defaultCapacity} and max capacity of {maxCapacity}!");

        // Create a new pool data entry
        poolData.Add(new ObjectPoolEntryData(prefab, defaultCapacity, maxCapacity));

        // If a pool for the prefab already exists, skip this
        _pools.TryAdd(prefab, pool);
        return;

        GameObject CreateAndConnectScript()
        {
            var obj = createFunc();

            // If the object doesn't have a script, raise an exception
            if (!obj.TryGetComponent(out TObjectType script))
                throw new MissingComponentException($"No {typeof(TObjectType).Name} script found on {obj.name}!");

            // Add the script to the dictionary
            _scripts.Add(obj, script);

            // Call the OnPoolCreate function
            _scripts[obj].OnPoolCreate();

            return obj;
        }

        void RemoveScriptOnDestroy(GameObject obj)
        {
            // Call the OnPoolDestroy function
            CallDestroyFunc(obj);

            _scripts.Remove(obj);
        }

        void CallGetFunc(GameObject obj)
        {
            // Add the object to the active game objects
            _activeGameObjects.Add(obj, prefab);

            // Call the OnPoolGet function
            _scripts[obj].OnPoolGet();
        }

        void CallReleaseFunc(GameObject obj)
        {
            // Remove the object from the active game objects
            _activeGameObjects.Remove(obj);

            // Call the OnPoolRelease function
            _scripts[obj].OnPoolRelease();
        }

        void CallDestroyFunc(GameObject obj)
        {
            _scripts[obj].OnPoolDestroy();

            // Remove the object from the dictionary
            _scripts.Remove(obj);
        }
    }

    public void RemovePool(GameObject prefab)
    {
        // If the pool doesn't exist, skip this
        if (!_pools.TryGetValue(prefab, out var pool))
            return;

        // Destroy all elements in the pool
        pool.Clear();

        // Remove the pool from the dictionary
        _pools.Remove(prefab);
    }

    public GameObject GetGameObject(GameObject prefab)
    {
        // If the pool already exists, return an object from it
        if (_pools.TryGetValue(prefab, out var pool))
            return pool.Get();

        throw new KeyNotFoundException($"No pool found for {prefab.name}!");
    }

    public TObjectType GetScript(GameObject actualObject)
    {
        // If the script already exists, return it
        if (_scripts.TryGetValue(actualObject, out var script))
            return script;

        throw new KeyNotFoundException($"No script found for {actualObject.name}!");
    }

    public void ReleaseGameObject(GameObject actualObject)
    {
        // If the object is active, release it
        if (_activeGameObjects.TryGetValue(actualObject, out var prefab))
            _pools[prefab].Release(actualObject);
    }

    #endregion
}