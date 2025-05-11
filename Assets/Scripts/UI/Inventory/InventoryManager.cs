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

    void Start()
    {
        uiManager = GameObject.FindFirstObjectByType<UIManager>();
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            inventorySlots = GetComponentsInChildren<InventorySlot>();
            Debug.Log("Auto-filled inventory slots: " + inventorySlots.Length);
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
        Item runtimeItem = item.Clone(); // Clone to avoid modifying the original asset
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

        InventorySlot chestSlot = parent.GetComponentInParent<InventorySlot>();
        if (!inventorySlots.Contains(chestSlot))
        {
            // Force place in first free inventory slot
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click moved external item {item.item.itemName} to inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free inventory slot for external item.");
            return;
        }

        // Case 0,5: Item is from player inventory, but CoreInventory is disabled
        if (uiManager.coreInventory != null && !uiManager.coreInventory.activeInHierarchy)
        {
            foreach (InventorySlot slot in chestSlots)
            {
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click moved item {item.item.itemName} to external inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free external inventory slot found.");
            return;
        }

        // Case 1: Item is in EquipmentSlot
        EquipmentSlot equipmentSlot = parent.GetComponentInParent<EquipmentSlot>();
        if (equipmentSlot != null)
        {
            equipmentSlot.equipmentManager?.Unequip(item.item);

            // Try to move to first free inventory slot
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click unequipped and moved {item.item.itemName} to inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free inventory slot to move equipped item.");
            return;
        }

        // Case 2: Item is in HotbarSlot
        HotbarSlot hotbarSlot = parent.GetComponentInParent<HotbarSlot>();
        if (hotbarSlot != null)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    item.transform.SetParent(slot.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.parentAfterDrag = slot.transform;

                    Debug.Log($"Shift-click moved hotbar item {item.item.itemName} to inventory.");
                    return;
                }
            }

            Debug.LogWarning("No free inventory slot to move hotbar item.");
            return;
        }

        // Case 3: Item is in inventory, try to equip or put in hotbar
        if (item.item is EquipmentItem equipment)
        {
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
                        slot.equipmentManager?.Equip(item.item);

                        Debug.Log($"Shift-click equipped {item.item.itemName}.");
                        return;
                    }
                    else
                    {
                        // Swap
                        slot.equipmentManager?.Unequip(existing.item);
                        existing.transform.SetParent(parent);
                        existing.transform.localPosition = Vector3.zero;

                        item.transform.SetParent(slot.transform);
                        item.transform.localPosition = Vector3.zero;
                        item.parentAfterDrag = slot.transform;

                        slot.equipmentManager?.Equip(item.item);

                        Debug.Log($"Shift-click swapped {item.item.itemName} with {existing.item.itemName}.");
                        return;
                    }
                }
            }
        }
        else
        {
            // Try to place in first available HotbarSlot
            foreach (HotbarSlot hotbar in hotbarSlots)
            {
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

}
