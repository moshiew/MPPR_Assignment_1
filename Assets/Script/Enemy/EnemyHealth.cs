using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyHealthBar healthBar;

    public int maxHealth = 30; // Health value for the enemy.
    public int currentHealth; // Current health value of the enemy.

    private void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();

        // Set the current health to maximum at the start.
        currentHealth = maxHealth;

        // Initialize the health bar with the maximum health value.
        healthBar.setMaxHealth(currentHealth);
    }

    private void Update()
    {
        HealthCheck();
    }

    /// <summary>
    /// Checks if the enemy's health is zero and destroys the enemy object if true.
    /// </summary>
    private void HealthCheck()
    {
        if (currentHealth <= 0)
        {
            Destroy(transform.root.gameObject); // Destroy the root object of the enemy.
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}
