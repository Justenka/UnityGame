using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Player player;

    public void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseHealingPotion();
        }
    }
    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++) // checking if there are any slots that have the current item that is not max count
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
        for (int i = 0; i < inventorySlots.Length; i++)// checking for any empty slot
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
    public void UseHealingPotion()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.type == ItemType.Consumable && itemInSlot.item.actionType == ActionType.Consumable)
            {
                player.UseHealth(-itemInSlot.item.healAmount); // Healing by negating damage
                RemoveItem(itemInSlot);
                Debug.Log($"Used {itemInSlot.item.itemName}, restored {itemInSlot.item.healAmount} HP.");
                return;
            }
        }

        Debug.Log("No healing potions available!");
    }

    void RemoveItem(InventoryItem itemInSlot)
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
    }
}