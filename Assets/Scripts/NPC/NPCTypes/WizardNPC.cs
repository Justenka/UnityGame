using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardNPC : NPCBase
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
        if (storageInventory == null)
        {
            storageInventory = FindInactiveObjectWithTag("StorageInventory");
        }

        if (scene.name != "FirstDungeon")
        {
            npcUI = FindInactiveObjectWithTag("WizardUI");
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
    }
}