using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ObjectPoolEntryData
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private int countAll;
    [SerializeField] private int countActive;
    [SerializeField] private int countInactive;

    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxCapacity;

    #region Getters & Setters

    public GameObject Prefab
    {
        get => prefab;
        set => prefab = value;
    }

    public int CountAll
    {
        get => countAll;
        set => countAll = value;
    }

    public int CountActive
    {
        get => countActive;
        set => countActive = value;
    }

    public int CountInactive
    {
        get => countInactive;
        set => countInactive = value;
    }

    public int DefaultCapacity
    {
        get => defaultCapacity;
        set => defaultCapacity = value;
    }

    public int MaxCapacity
    {
        get => maxCapacity;
        set => maxCapacity = value;
    }

    public ObjectPoolEntryData(GameObject prefab, int defaultCapacity, int maxCapacity)
    {
        this.prefab = prefab;
        this.defaultCapacity = defaultCapacity;
        this.maxCapacity = maxCapacity;
    }

    #endregion
}