using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public float damage;

    public float score;

    public float speed;

    public void ChangeHealth(float amount)
    {
        health += amount;

        if (health <= 0)
            Destroy(gameObject);
    }
}