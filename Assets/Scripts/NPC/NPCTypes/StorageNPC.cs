using System;
using UnityEngine;

public class StorageNPC : NPCBase
{
    public GameObject storageInventory;
    public static event Action OnChestOpened;
    public override void Interact()
    {
        OpenUI();
        if (!storageInventory.activeSelf) {
            storageInventory.SetActive(true);
            OnChestOpened?.Invoke();
            UIManager.Instance.RegisterOpenMenu(storageInventory);
        }
    }
}