using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public InventorySlot[] chestSlots;
    public HotbarSlot[] hotbarSlots;
    public EquipmentSlot[] equipmentSlots;
    public GameObject inventoryItemPrefab;
    public Player player;
    private UIManager uiManager;
    public GameObject storageUIPanel;
    public GameObject playerGridInventoryPanel;

    void Start()
    {
        uiManager = GameObject.FindFirstObjectByType<UIManager>();
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            inventorySlots = GetComponentsInChildren<InventorySlot>();
            Debug.Log("Auto-filled inventory slots: " + inventorySlots.Length);
        }

        EquipmentManager playerEquipmentManager = null;
        if (player != null)
        {
            playerEquipmentManager = player.GetComponent<EquipmentManager>();
            if (playerEquipmentManager == null)
            {
                Debug.LogError("Player GameObject found, but no EquipmentManager component on it!");
            }
        }
        else
        {
            Debug.LogError("Player reference is null in InventoryManager!");
        }


        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot != null)
            {
                // Assign if not already assigned
                if (slot.equipmentManager == null)
                {
                    slot.equipmentManager = playerEquipmentManager;
                }
                if (slot.equipmentManager == null) // Double check after attempt
                {
                    Debug.LogError($"EquipmentManager for slot {slot.name} is still null after assignment attempt!");
                }
            }
        }
    }

    public bool AddItem(Item item, int count)
    {
        // Try to stack first
        for (int i = 0; i < inventorySlots.Length && count > 0; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.IsSameItem(item) && itemInSlot.count < maxStackedItems)
            {
                int spaceLeft = maxStackedItems - itemInSlot.count;
                int toAdd = Mathf.Min(spaceLeft, count);
                itemInSlot.count += toAdd;
                itemInSlot.RefreshCount();
                count -= toAdd;
            }
        }

        // Then create new stacks
        for (int i = 0; i < inventorySlots.Length && count > 0; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                int toAdd = Mathf.Min(maxStackedItems, count);
                Item runtimeItem = item.Clone();
                GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
                InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
                inventoryItem.InitialiseItem(runtimeItem);
                inventoryItem.count = toAdd;
                inventoryItem.RefreshCount();
                count -= toAdd;
            }
        }

        if (count > 0)
        {
            Debug.LogWarning("Inventory full — some items couldn't be returned.");
            return false;
        }

        return true;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        Item runtimeItem = item.Clone();
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(runtimeItem);
    }

    public void RemoveItem(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                return;
            }
        }
    }

    public void ClearInventory()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }
    }

    //Handles Shift click item moving
    public void SmartShiftMove(InventoryItem item)
    {
        Transform parent = item.transform.parent;

        // Check if the item is in a ChestSlot (an external inventory)
        InventorySlot chestSlot = parent.GetComponentInParent<InventorySlot>();
        // It implies the item is coming from an external source like a chest.
        if (chestSlots.Contains(chestSlot) && storageUIPanel != null && storageUIPanel.activeInHierarchy)
        {
            Debug.Log($"Item {item.item.itemName} is in a chest slot. Attempting to move to player inventory.");
            // Force place in first free inventory slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click moved external item {item.item.itemName} to inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free inventory slot for external item. Item remains in chest.");
            return;
        }

        // Case 0,5: Item is from player inventory.
        // This implies moving from player inventory to chest.
        if (uiManager.coreInventory != null && !uiManager.coreInventory.activeInHierarchy &&
            storageUIPanel != null && storageUIPanel.activeInHierarchy)
        {
            Debug.Log($"Item {item.item.itemName} is in player inventory, and core inventory is disabled. Attempting to move to chest.");
            for (int i = 0; i < chestSlots.Length; i++)
            {
                InventorySlot slot = chestSlots[i];
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click moved item {item.item.itemName} from inventory to external (chest) inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free external inventory slot found. Item stays in player inventory.");
            return;
        }


        // Case 1: Item is in EquipmentSlot
        EquipmentSlot equipmentSlot = parent.GetComponentInParent<EquipmentSlot>();
        if (equipmentSlot != null)
        {
            Debug.Log($"Attempting to move item {item.item.itemName} from an EquipmentSlot.");
            if (equipmentSlot.equipmentManager == null)
            {
                Debug.LogError($"EquipmentManager is NULL on EquipmentSlot: {equipmentSlot.name}! Cannot unequip.");
            }
            else
            {
                Debug.Log($"Calling Unequip for {item.item.itemName} on EquipmentManager: {equipmentSlot.equipmentManager.name}");
                equipmentSlot.equipmentManager.Unequip(item.item);
            }


            // Try to move to first free inventory slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click unequipped and moved {item.item.itemName} to inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free inventory slot to move equipped item. Item remains in equipment slot.");
            return;
        }

        // Case 2: Item is in HotbarSlot
        HotbarSlot hotbarSlot = parent.GetComponentInParent<HotbarSlot>();
        if (hotbarSlot != null)
        {
            Debug.Log($"Attempting to move item {item.item.itemName} from a HotbarSlot.");
            // Try to move to first free inventory slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click moved hotbar item {item.item.itemName} to inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free inventory slot to move hotbar item. Item remains in hotbar.");
            return;
        }

        // Case 3: Item is in inventory, try to equip or put in hotbar
        InventorySlot currentInventorySlot = parent.GetComponentInParent<InventorySlot>();
        if (inventorySlots.Contains(currentInventorySlot)) // Confirm it's from main inventory
        {
            Debug.Log($"Attempting to move item {item.item.itemName} from inventory.");
            if (item.item is EquipmentItem equipment)
            {
                if (playerGridInventoryPanel == null || !playerGridInventoryPanel.activeInHierarchy)
                {
                    Debug.LogWarning($"Cannot unequip {item.item.itemName}. PlayerGridInventory is not active.");
                    return;
                }
                Debug.Log($"Item is an EquipmentItem. Trying to equip.");
                foreach (var slot in equipmentSlots)
                {
                    if (slot.acceptedEquipmentType == equipment.equipmentType)
                    {
                        InventoryItem existing = slot.GetComponentInChildren<InventoryItem>();
                        if (existing == null)
                        {
                            item.transform.SetParent(slot.transform);
                            item.transform.localPosition = Vector3.zero;
                            item.parentAfterDrag = slot.transform;
                            if (slot.equipmentManager == null)
                            {
                                Debug.LogError($"EquipmentManager is NULL on target EquipmentSlot: {slot.name}! Cannot equip.");
                            }
                            else
                            {
                                Debug.Log($"Calling Equip for {item.item.itemName} on EquipmentManager: {slot.equipmentManager.name}");
                                slot.equipmentManager.Equip(item.item);
                            }

                            Debug.Log($"Shift-click equipped {item.item.itemName}.");
                            return;
                        }
                        else
                        {
                            // Swap
                            Debug.Log($"Swapping equipped item: {existing.item.itemName} with {item.item.itemName}");
                            if (slot.equipmentManager == null)
                            {
                                Debug.LogError($"EquipmentManager is NULL on target EquipmentSlot: {slot.name}! Cannot swap.");
                            }
                            else
                            {
                                Debug.Log($"Calling Unequip for {existing.item.itemName}");
                                slot.equipmentManager.Unequip(existing.item); // Unequip the old item
                            }

                            existing.transform.SetParent(parent);
                            existing.transform.localPosition = Vector3.zero;
                            existing.parentAfterDrag = parent; // Ensure old item goes back to original slot

                            item.transform.SetParent(slot.transform);
                            item.transform.localPosition = Vector3.zero;
                            item.parentAfterDrag = slot.transform;

                            if (slot.equipmentManager == null)
                            {

                            }
                            else
                            {
                                Debug.Log($"Calling Equip for {item.item.itemName}");
                                slot.equipmentManager.Equip(item.item); // Equip the new item
                            }

                            Debug.Log($"Shift-click swapped {item.item.itemName} with {existing.item.itemName}.");
                            return;
                        }
                    }
                }
                Debug.LogWarning($"No suitable equipment slot found for {item.item.itemName}.");
            }
            else // Not an equipment item, try hotbar
            {
                Debug.Log($"Item is not an EquipmentItem. Trying to move to hotbar.");
                // Try to place in first available HotbarSlot
                for (int i = 0; i < hotbarSlots.Length; i++) // Use for loop for early exit
                {
                    HotbarSlot hotbar = hotbarSlots[i];
                    if (hotbar.GetComponentInChildren<InventoryItem>() == null)
                    {
                        item.transform.SetParent(hotbar.transform);
                        item.transform.localPosition = Vector3.zero;
                        item.parentAfterDrag = hotbar.transform;

                        Debug.Log($"Shift-click moved {item.item.itemName} to hotbar.");
                        return;
                    }
                }

                Debug.Log("No hotbar slot available. Item stays in inventory.");
            }
        }
        else
        {
            Debug.LogWarning($"Shift-click: Item {item.item.itemName} is not in a recognized inventory, hotbar, or equipment slot.");
        }
    }
}