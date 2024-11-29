using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    public int enemyHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0) 
        { 
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    { 
        enemyHealth -= damage;
        enemyHealth = Mathf.Clamp(enemyHealth, 0, 20);
    }

}
