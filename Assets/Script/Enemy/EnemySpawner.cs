using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefabs; // All enemy prefab

    [Header("Enemy Spawn")]
    [SerializeField] public List<Transform> waypointsList;  // List of waypoints for the enemy to follow
    private float elapsedTime;
    [SerializeField] private float enemyPerSecond = 2f; // Time between enemy spawn
    [SerializeField] private Transform spawnPoint;

    [Header("Wave Control")]
    private int waveCount = 1;
    [SerializeField] private float timeBetweenWaves = 5f;
    private bool isSpawning = false;
    private float enemiesAlive;
    private int enemiesLeftToSpawn;

    [Header("Difficulty Scalar")]
    [SerializeField] private int baseEnemies = 5;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroyed = new UnityEvent();

    private void Awake()
    {
        onEnemyDestroyed.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
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

        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    /// <summary>
    /// Start to call the first wave
    /// </summary>
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave()
    {
        isSpawning = false;
        elapsedTime = 0;
        waveCount += 1;
        StartCoroutine(StartWave());
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
        GameObject prefabToInstantiate = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefabToInstantiate, spawnPoint.position, Quaternion.identity);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
}
