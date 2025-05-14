using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public GameObject[] tutorialMessages;
    private int currentMessageIndex = 0;
    private int enemiesAlive = 0;
    private bool playerMoveReady = false;
    private bool playerMoved = false;
    private float moveTime = 0f;
    private bool waitingToAdvance = false;

    private void Start()
    {
        ShowMessage(currentMessageIndex);
    }
    private void Update()
    {
        if (waitingToAdvance && Time.time - moveTime >= 10f)
        {
            waitingToAdvance = false;
            Debug.Log("10 seconds passed since movement. Advancing tutorial.");
            AdvanceTutorialMessage();
        }
    }
    private void ShowMessage(int index)
    {
        for (int i = 0; i < tutorialMessages.Length; i++)
        {
            tutorialMessages[i].SetActive(i == index);
        }
    }
    public void AdvanceTutorialMessage()
    {
        currentMessageIndex++;
        Time.timeScale = 0;
        if (currentMessageIndex < tutorialMessages.Length)
        {
            ShowMessage(currentMessageIndex);
        }
        else
        {
            Debug.Log("Tutorial messages finished. Completing tutorial.");
            CompleteTutorial();
        }
    }
    private void OnEnable()
    {
        PlayerMovement.OnPlayerMoved += HandlePlayerMoved;
        Enemy.OnEnemyDied += HandleEnemyDeath;
        TutorialNPC.OnChestOpened += HandleChestOpened;
        EquipmentManager.OnWeaponEquipped += HandleWeaponEquipped;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerMoved -= HandlePlayerMoved;
        Enemy.OnEnemyDied -= HandleEnemyDeath;
        TutorialNPC.OnChestOpened -= HandleChestOpened;
        EquipmentManager.OnWeaponEquipped -= HandleWeaponEquipped;
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        Debug.Log("Enemy died. Enemies left: " + enemiesAlive);

        if (enemiesAlive <= 0)
        {
            Debug.Log("All enemies defeated.");
            AdvanceTutorialMessage();
        }
    }
    public void CompleteTutorial()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }
    public void SetReadyToMove()
    {
        playerMoveReady = true;
    }
    public void SpawnOneEnemy()
    {
        enemiesAlive++;
        Instantiate(enemyPrefab, spawnPoints[0].position, Quaternion.identity);
        Debug.Log("Spawned enemy at: " + spawnPoints[0].position);
    }
    public void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
            {
                Debug.LogWarning("A spawn point is null, skipping...");
                continue;
            }
            enemiesAlive++;
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Spawned enemy at: " + spawnPoint.position);
        }
    }
    public void SetTime(int time)
    {
        Time.timeScale = time;
    }
    private void HandlePlayerMoved()
    {
        if (!playerMoved && playerMoveReady)
        {
            playerMoved = true;
            moveTime = Time.time;
            waitingToAdvance = true;
            Debug.Log("Player moved! Starting tutorial timer.");
        }
    }
    private void HandleChestOpened()
    {
        if (currentMessageIndex == 2) // Adjust this to match your chest tutorial message index
        {
            Debug.Log("Chest opened. Advancing tutorial.");
            AdvanceTutorialMessage();
        }
    }
    private void HandleWeaponEquipped()
    {
        AdvanceTutorialMessage();
    }
}
