using System;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [SerializeField] [Min(0)] private float maxHealth = 5;
    [SerializeField] [Min(0)] private float currentHealth;

    public event Action<Actor> OnHit;

    #region Getters

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
}