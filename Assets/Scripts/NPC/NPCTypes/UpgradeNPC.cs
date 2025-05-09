using UnityEngine;

public class UpgradeNPC : NPCBase
{
    public GameObject storageInventory;
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