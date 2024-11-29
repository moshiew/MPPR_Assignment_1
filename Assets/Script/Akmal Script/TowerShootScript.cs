using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerShootScript : MonoBehaviour
{
    public GameObject towerBulletPrefab;
    public float timeIntervals = 3.0f;
    public float elapsedTime;
    public static Transform enemyPosition;
    public GameObject enemy;
    public Transform towerPosition;
    public float detectionRange = 15f;
    public LayerMask obstacleLayer;
   
    
   
    // Update is called once per frame
    void Update()
    {

        if (enemy == null)
        {
            enemy = FindClosestEnemy(); // When no enemy is detected, try to find closest enemy
            
        }
        else
        {
            if (elapsedTime >= timeIntervals && isEnemyWithinRange(enemy.transform))
            {
               
                //When enemy is found and enemy is within the player range and after passing getting to the time interval, set the positions for the bullet after spawn.
                towerPosition = gameObject.transform;
                enemyPosition = enemy.transform;

                // Instantiate the bullet

                GameObject bullet = Instantiate(towerBulletPrefab, towerPosition.position, towerPosition.rotation);

                // Pass the current enemy reference to the bullet's EasingMovement script
                BulletMovementScript movement = bullet.GetComponent<BulletMovementScript>();
                if (movement != null)
                {
                    movement.currentenemy = enemy; // Pass the enemy GameObject
                    

                }
               

                elapsedTime = 0;
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
        }

        
    }
    GameObject FindClosestEnemy()
    {
        //Function to find the closest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Use array to get all the enemy in the game scene
        
        GameObject currentEnemy = null;

    float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            //After checking all the enemy, try to find the enemy closest to the tower
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
            
            if (distanceToEnemy < shortestDistance && isEnemyWithinRange(enemy.transform))
            {
                shortestDistance = distanceToEnemy;
                
                currentEnemy = enemy;
            }
        }
        //The closest enemy will be the current enemy
        return currentEnemy;
        
    }

    bool isEnemyWithinRange(Transform target)
    {
        // This is to find the closest enemy/target using raycast to detect
        Vector3 directionToTarget = (target.position - this.transform.position).normalized;
        
        // Perform a raycast to check for obstacles
        if (Physics.Raycast(this.transform.position, directionToTarget, out RaycastHit hit, detectionRange, obstacleLayer))
        {
            // If the ray hits the target, it is visible
            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;

    }


}
