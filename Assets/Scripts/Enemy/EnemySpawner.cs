using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public float timeToSpawn;
    private float spawnCounter;
    public float spawnDistance = 20f;
    private Transform target;
    private GameObject player;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnCounter = timeToSpawn;
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            target = player.GetComponent<Transform>();
        }
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeToSpawn;
            Vector3 spawnPosition = GetSpawnPositionOutsideView();
            Instantiate(enemyToSpawn, spawnPosition, transform.rotation);
        }
        transform.position = target.position;
    }
    Vector3 GetSpawnPositionOutsideView()
    {
        Vector3 spawnPosition;
        bool isVisible;
        do
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            spawnPosition = target.position + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnDistance;

            Vector3 viewportPos = mainCamera.WorldToScreenPoint(spawnPosition);
            isVisible = viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0;

        }
        while (isVisible);
        return spawnPosition;
    }
}
