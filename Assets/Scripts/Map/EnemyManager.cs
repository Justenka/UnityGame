using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private int totalEnemies = 0;
    public GameObject portal; // Assign via Inspector or Find()

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddEnemies(int count)
    {
        totalEnemies += count;
        Debug.Log($"[EnemyManager] Added {count} enemies. Total now: {totalEnemies}");
    }

    public void EnemyDied()
    {
        totalEnemies--;
        Debug.Log($"[EnemyManager] Enemy died. Remaining: {totalEnemies}");

        if (totalEnemies <= 0)
        {
            Debug.Log("[EnemyManager] All enemies defeated. Activating portal.");
            if (portal != null)
                portal.SetActive(true);
            else
                Debug.LogWarning("[EnemyManager] Portal not assigned!");
        }
    }
}
