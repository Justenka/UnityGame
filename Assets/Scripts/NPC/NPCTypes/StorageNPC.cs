using UnityEngine;

public class StorageNPC : NPCBase
{
    public GameObject storageInventory;
    public override void Interact()
    {
        OpenUI();
        if (!storageInventory.activeSelf) {
            storageInventory.SetActive(true);
            UIManager.Instance.RegisterOpenMenu(storageInventory);
        }
    }
}