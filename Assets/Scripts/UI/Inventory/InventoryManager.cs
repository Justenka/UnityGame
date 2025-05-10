using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public HotbarSlot[] hotbarSlots;
    public EquipmentSlot[] equipmentSlots;
    public GameObject inventoryItemPrefab;
    public Player player;

    void Start()
    {
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            inventorySlots = GetComponentsInChildren<InventorySlot>();
            Debug.Log("Auto-filled inventory slots: " + inventorySlots.Length);
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        Debug.LogWarning("Inventory is full, item could not be added.");
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
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
