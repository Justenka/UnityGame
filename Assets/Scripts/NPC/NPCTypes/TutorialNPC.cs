using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialNPC : NPCBase
{
    public GameObject storageInventory;

    public static event Action OnChestOpened;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        storageInventory = FindInactiveObjectWithTag("StorageInventory");
        npcUI = FindInactiveObjectWithTag("TutorialUI");
    }

    private void Update()
    {
        if (storageInventory == null)
        {
            storageInventory = FindInactiveObjectWithTag("StorageInventory");
        }
    }

    GameObject FindInactiveObjectWithTag(string tag)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
                return obj;
        }
        return null;
    }

    public override void Interact()
    {
        OpenUI();
        if (!storageInventory.activeSelf)
        {
            storageInventory.SetActive(true);
            OnChestOpened?.Invoke();
            UIManager.Instance.RegisterOpenMenu(storageInventory);
        }
    }
}