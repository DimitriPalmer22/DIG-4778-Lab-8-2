using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Min(0)] private float maxHealth = 5;
    [SerializeField] [Min(0)] private float currentHealth;

    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;

    private void Start()
    {
        // Set the current health to the max health
        currentHealth = maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        // Return if the amount is 0
        if (amount == 0)
            return;

        // Change the health by the amount
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Update the text
        ScoreManager.Instance.UpdateText();

        // If the health is less than or equal to 0
        // Call the Die function
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // Set the game over text to active
        ScoreManager.Instance.SetGameOverText(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Try to get the enemy component
        if (!other.TryGetComponent<Enemy>(out var enemy))
            return;

        // Change the player's health by the enemy's damage
        ChangeHealth(-enemy.damage);

        // Destroy the enemy
        Destroy(enemy.gameObject);
    }
}