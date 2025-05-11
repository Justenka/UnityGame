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
    public override void CloseUI()
    {
        if (npcUI.TryGetComponent(out UpgradeUI upgradeUI))
        {
            upgradeUI.ReturnItemsToInventory();
        }

        base.CloseUI();
    }
}