using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private bool triggered = false;
    private EnemySpawners spawner;

    void Start()
    {
        spawner = GetComponentInChildren<EnemySpawners>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            spawner.SpawnEnemies();
            triggered = true;
        }
    }
}
public class EnemySpawners : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Transform[] spawnPoints;

    private bool hasSpawned = false;

    public void SpawnEnemies()
    {
        if (hasSpawned) return;

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
        }

        hasSpawned = true;
    }
    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }
}