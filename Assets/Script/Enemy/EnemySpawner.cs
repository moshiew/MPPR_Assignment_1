using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefabs; // All enemy prefab

    [Header("Enemy Spawn")]
    [SerializeField] public List<Transform> waypointsList;  // List of waypoints for the enemy to follow
    private float elapsedTime;
    private float enemyPerSecond = 2f; // Time between enemy spawn
    [SerializeField] private Transform spawnPoint;

    [Header("Wave Control")]
    private int waveCount = 1;
    private float timeBetweenWaves = 5f;
    private bool isSpawning = false;
    private float enemiesAlive;
    private int enemiesLeftToSpawn;

    [Header("Difficulty Scalar")]
    private int baseEnemies = 5;
    private float difficultyScalingFactor = 0.75f;


    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (!isSpawning) return; // Check if the wave have started

        elapsedTime += Time.deltaTime;

        if((elapsedTime >= enemyPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            elapsedTime = 0;
        }
    }

    /// <summary>
    /// Start to call the first wave
    /// </summary>
    private void StartWave()
    {
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    /// <summary>
    /// Calculate how much enemy to spawn next wave
    /// </summary>
    /// <returns>The number of how much enemy to be spawn next wave</returns>
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(waveCount, difficultyScalingFactor));
    }

    private void SpawnEnemy()
    {
        GameObject prefabToInstantiate = enemyPrefabs[0];
        Instantiate(prefabToInstantiate, spawnPoint.position, Quaternion.identity);
    }
}
