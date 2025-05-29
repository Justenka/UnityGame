using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Drag & drop enemy prefabs in the Inspector
    public Transform[] spawnPoints;   // Set these to specific spots in the room
    private bool hasSpawned = false;
    public bool bossDoor = false;

    private void Start()
    {
        GameObject roomParent = GameObject.Find("Rooms");
        if (roomParent == null)
        {
            Debug.Log("Error: Could not find 'Rooms' GameObject.");
            return;
        }

        int roomEnemyCount = 0;

        foreach (Transform room in roomParent.transform)
        {
            foreach (Transform child in room)
            {
                if (child.name.StartsWith("SpawnPoint"))
                    roomEnemyCount++;
            }
            Debug.Log($"{room.name} has {roomEnemyCount} spawn points.");
            EnemyManager.Instance?.AddEnemies(roomEnemyCount);
            Debug.Log($"Reseting EnemyCount: {roomEnemyCount}");
            roomEnemyCount = 0; // Reset for next room
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the room! Spawning enemies...");
            SpawnEnemies();
            hasSpawned = true;
        }

        // Optional boss room teleport (can be removed if unused)
        if (other.CompareTag("Player") && bossDoor)
        {
            GameObject player = GameObject.Find("Player");
            GameObject temp = GameObject.Find("BossSpawnRoom");
            if (player != null && temp != null)
                player.transform.position = temp.transform.position;
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Enemy prefabs not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawn points not assigned!");
            return;
        }

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null) continue;

            int randIndex = Random.Range(0, enemyPrefabs.Length);
            if (enemyPrefabs[randIndex] == null) continue;

            Instantiate(enemyPrefabs[randIndex], spawnPoint.position, Quaternion.identity);
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
        EnemyManager.Instance?.EnemyDied();
    }
}
