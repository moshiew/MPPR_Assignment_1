using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private EnemyHealthBar healthBar;

    public int maxHealth = 30;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(currentHealth);  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            currentHealth -= 5;
            healthBar.setHealth(currentHealth);
        }
    }
}