using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public ConsumableType consumableType;
    public Player player;

    public void Start()
    {

    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    UseHealthPotion();
        //}
    }
    public bool AddItem(Item item)
    {
        // First, check if we already have this item in the inventory and if it is stackable
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount(); // Update UI count
                return true;
            }
        }

        // Then, check for any empty slot
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

        // If no free slot or room to stack, return false
        Debug.LogWarning("Inventory is full, item could not be added.");
        return false;
    }
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
    //public void UseHealthPotion()
    //{
    //    foreach (var slot in inventorySlots)
    //    {
    //        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
    //        if (itemInSlot != null && itemInSlot.item is ConsumableItem consumable &&
    //            consumable.consumableType == ConsumableType.Health)
    //        {
    //            consumable.Use(player.gameObject);
    //            RemoveItem(itemInSlot);
    //            return;
    //        }
    //    }

    //    Debug.Log("No health potions available!");
    //}

    //void RemoveItem(InventoryItem itemInSlot)
    //{
    //    itemInSlot.count--;
    //    if (itemInSlot.count <= 0)
    //    {
    //        Destroy(itemInSlot.gameObject);
    //    }
    //    else
    //    {
    //        itemInSlot.RefreshCount();
    //    }
    //}
}