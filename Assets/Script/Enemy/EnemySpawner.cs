using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn from.

    [Header("Enemy Spawn")]
    [SerializeField] public List<Transform> waypointsList; // List of waypoints that the enemies will follow.
    private float elapsedTime; // Tracks time passed since the last enemy spawn.
    [SerializeField] private float enemyPerSecond = 2f; // Time interval between spawns (1/enemyPerSecond = spawn rate).
    [SerializeField] private Transform spawnPoint; // Location where enemies will be spawned.

    [Header("Wave Control")]
    public int waveCount = 1;
    [SerializeField] private float timeBetweenWaves = 5f; // Delay before starting the next wave.
    private bool isSpawning = false; // Indicates whether enemies are currently spawning.
    private float enemiesAlive; // Tracks the number of enemies currently alive.
    private int enemiesLeftToSpawn; // Tracks the remaining enemies to spawn in the current wave.

    [Header("Difficulty Scalar")]
    [SerializeField] private int baseEnemies = 5; // Base number of enemies in the first wave.
    [SerializeField] private float difficultyScalingFactor = 0.75f; // Determines the exponential scaling of enemies per wave.

    [Header("Events")]
    public static UnityEvent onEnemyDestroyed = new UnityEvent(); // Event triggered when an enemy is destroyed.

    private void Awake()
    {
        // Subscribe to the onEnemyDestroyed event to decrement the enemiesAlive counter.
        onEnemyDestroyed.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        // Start the first wave after initialization.
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        // If not currently spawning enemies, exit early.
        if (!isSpawning) return;

        // Increment elapsed time by the time since the last frame.
        elapsedTime += Time.deltaTime;

        // Spawn an enemy if enough time has passed and there are remaining enemies to spawn.
        if (elapsedTime >= enemyPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--; // Decrease the remaining enemies to spawn.
            enemiesAlive++; // Increase the count of enemies currently alive.
            elapsedTime = 0; // Reset the elapsed time counter.
        }

        // Check if the wave has ended (no enemies alive and none left to spawn).
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    /// <summary>
    /// Starts a new wave of enemies after a delay.
    /// </summary>
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Wait for the wave delay.
        isSpawning = true; // Start spawning enemies.
        enemiesLeftToSpawn = EnemiesPerWave(); // Calculate the number of enemies for this wave.
    }

    /// <summary>
    /// Ends the current wave and prepares for the next wave.
    /// </summary>
    private void EndWave()
    {
        isSpawning = false; // Stop spawning enemies.
        elapsedTime = 0; // Reset the elapsed time counter.
        waveCount += 1; // Increment the wave count.
        StartCoroutine(StartWave()); // Start the next wave.
    }

    /// <summary>
    /// Calculates the number of enemies to spawn for the current wave.
    /// </summary>
    /// <returns>The number of enemies to spawn in the current wave.</returns>
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(waveCount, difficultyScalingFactor)); // The number scales exponentially based on the wave count and a difficulty factor.
    }

    /// <summary>
    /// Spawns a random enemy at the designated spawn point.
    /// </summary>
    private void SpawnEnemy()
    {
        // Choose a random enemy prefab from the list.
        GameObject prefabToInstantiate = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        // Instantiate the enemy prefab at the spawn point with no rotation.
        Instantiate(prefabToInstantiate, spawnPoint.position, Quaternion.identity);
    }

    /// <summary>
    /// Decrements the count of alive enemies.
    /// </summary>
    private void EnemyDestroyed()
    {
        enemiesAlive--; // Decrease the count of alive enemies.
    }
}
