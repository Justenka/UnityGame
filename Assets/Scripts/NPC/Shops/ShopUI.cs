using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Player player;
    public InventoryToggle inventoryUI;
    public ShopItemSlot[] shopSlots;
    public Item[] itemsForSale;
    public GameObject inventoryItemPrefab;
    public InventoryManager inventoryManager;

    protected virtual void OnEnable()
    {
        PopulateShop();

        if (inventoryUI != null)
            inventoryUI.OpenInventory();
    }

    private void PopulateShop()
    {
        Debug.Log($"Populating shop with items... List count: {itemsForSale.Length}");
        Debug.Log("Populating shop with items...");

        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i < itemsForSale.Length)
            {
                Debug.Log($"Adding item to slot: {itemsForSale[i].itemName}");

                shopSlots[i].Setup(itemsForSale[i], this);
                shopSlots[i].gameObject.SetActive(true);
            }
            else
            {
                shopSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public virtual void TryBuyItem(Item item)
    {
        if (player.currencyHeld >= item.price)
        {
            player.RemoveCurrency(item.price);
            AddItemToInventory(item);
            Debug.Log($"Bought {item.itemName} for {item.price}g.");
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    private void AddItemToInventory(Item item)
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is not assigned!");
            return;
        }

        if (inventoryManager.AddItem(item))
        {
            Debug.Log($"Item {item.itemName} added to inventory.");
        }
        else
        {
            Debug.LogWarning("Inventory is full, could not add item.");
        }
    }
}
