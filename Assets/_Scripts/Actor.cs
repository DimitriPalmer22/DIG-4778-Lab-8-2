using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Actor : MonoBehaviour, ISaveable
{
    public string SaveID => $"{gameObject.GetInstanceID()}";

    [SerializeField] [Min(0)] private float maxHealth = 5;
    [SerializeField] [Min(0)] private float currentHealth;

    public event Action<Actor> OnHit;

    #region Getters

    public abstract ActorData ActorData { get; }

    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;

    #endregion

    private void Awake()
    {
        CustomAwake();
    }

    protected virtual void CustomAwake()
    {
    }

    private void Start()
    {
        // Set the current health to the max health
        currentHealth = maxHealth;

        CustomStart();
    }

    protected virtual void CustomStart()
    {
    }

    #region Health Management

    private void ChangeHealth(float amount)
    {
        // Return if the amount is 0
        if (amount == 0)
            return;

        // Change the health by the amount
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // If the health is less than or equal to 0
        // Call the Die function
        if (currentHealth <= 0)
            Die();
    }

    public void TakeDamage(float damage)
    {
        // Change the health by the damage
        ChangeHealth(-damage);

        // Call the OnHit event
        OnHit?.Invoke(this);
    }

    public void Heal(float amount)
    {
        // Change the health by the amount
        ChangeHealth(amount);
    }

    public void SetHealth(float cHealth, float mHealth)
    {
        // Set the current health to the max health
        currentHealth = cHealth;
        maxHealth = mHealth;
    }

    protected abstract void Die();

    #endregion

    public void Load(string path)
    {
        CustomLoad(path);
    }

    protected abstract void CustomLoad(string path);
}

[Serializable]
public abstract class ActorData
{
    [SerializeField] private TransformSaver transform;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    public ActorData(Actor actor)
    {
        transform = (TransformSaver)actor.transform;
        currentHealth = actor.CurrentHealth;
        maxHealth = actor.MaxHealth;
    }
}