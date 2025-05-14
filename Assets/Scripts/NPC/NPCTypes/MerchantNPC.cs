using UnityEngine;
using UnityEngine.SceneManagement;

public class MerchantNPC : NPCBase
{
    public GameObject storageInventory;

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
        npcUI = FindInactiveObjectWithTag("ShopUI");
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
            UIManager.Instance.RegisterOpenMenu(storageInventory);
        }
    }
}