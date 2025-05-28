using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private bool hasSpawned = false;
    public bool bossDoor = false;
    public GameObject portal;

    private int spawnCount = 0;

    private void Start()
    {
        // Count only valid spawn points assigned
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
                spawnCount++;
        }

        Debug.Log($"Room has {spawnCount} spawn points.");

        if (portal != null)
            portal.SetActive(false); // Hide portal until enemies are defeated
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the room! Spawning enemies...");
            SpawnEnemies();
            hasSpawned = true;
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.Log("Enemy Prefabs array is empty or null!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.Log("Spawn Points array is empty or null!");
            return;
        }

        spawnCount = 0; // Reset count to match actual spawns

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
                continue;

            int randIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[randIndex];

            if (enemyToSpawn == null)
                continue;

            Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            spawnCount++;
            Debug.Log("Spawned enemy at: " + spawnPoint.position);
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        spawnCount--;
        Debug.Log("Enemy died. Enemies left: " + spawnCount);

        if (spawnCount <= 0)
        {
            Debug.Log("All enemies defeated!");

            if (portal != null)
            {
                portal.SetActive(true); // Enable portal collider/visuals
                Debug.Log("Portal activated!");
            }
            else
            {
                Debug.LogWarning("Portal reference is missing!");
            }
        }
    }
}
