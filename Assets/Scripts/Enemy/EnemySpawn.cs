using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Drag & drop enemy prefabs in the Inspector
    public Transform[] spawnPoints;   // Set these to specific spots in the room
    private bool hasSpawned = false;
    int spawnCount = 0;
    
    private void Start()
    {
        GameObject roomParent = GameObject.Find("Rooms");
        if(roomParent == null)
        {
            Debug.Log("Error with find Rooms");
            return;
        }
        foreach (Transform room in roomParent.transform)
        {
            foreach (Transform child in room)
            {
                if(child.name.StartsWith("SpawnPoint"))
                    spawnCount++;
            }
            Debug.Log($"{room.name} has {spawnCount} spawn points");
        }
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
        // Check if arrays are valid
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy Prefabs array is empty or null!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn Points array is empty or null!");
            return;
        }

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
            {
                Debug.LogWarning("A spawn point is null, skipping...");
                continue;
            }

            int randIndex = Random.Range(0, enemyPrefabs.Length);
            if (enemyPrefabs[randIndex] == null)
            {
                Debug.LogWarning("Enemy prefab at index " + randIndex + " is null, skipping...");
                continue;
            }

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
        spawnCount--;
        Debug.Log("Enemy died. Enemies left: " + spawnCount);

        if (spawnCount <= 0)
        {
            Debug.Log("All enemies defeated.");
            
        }
    }
}