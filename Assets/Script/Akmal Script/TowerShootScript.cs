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
   
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (enemy == null)
        {
            enemy = FindClosestEnemy();
            
        }
        else
        {
            if (elapsedTime >= timeIntervals && isEnemyWithinRange(enemy.transform))
            {
               

                towerPosition = gameObject.transform;
                enemyPosition = enemy.transform;

                // Instantiate the bullet

                GameObject bullet = Instantiate(towerBulletPrefab, towerPosition.position, towerPosition.rotation);

                // Pass the current enemy reference to the bullet's EasingMovement script
                EasingMovement easingMovement = bullet.GetComponent<EasingMovement>();
                if (easingMovement != null)
                {
                    easingMovement.currentenemy = enemy; // Pass the enemy GameObject
                    

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject currentEnemy = null;

    float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
            
            if (distanceToEnemy < shortestDistance && isEnemyWithinRange(enemy.transform))
            {
                shortestDistance = distanceToEnemy;
                
                currentEnemy = enemy;
            }
        }

        return currentEnemy;
        
    }

    bool isEnemyWithinRange(Transform target)
    {
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
