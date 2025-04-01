using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Player player;
    public InventoryToggle inventoryUI;
    public ShopItemSlot[] shopSlots; // Fixed 3 slots
    public Item[] itemsForSale; // Drag in 3 items in Inspector
    public GameObject inventoryItemPrefab;
    public InventoryManager inventoryManager;

    private void OnEnable()
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

    public void TryBuyItem(Item item)
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

    //private void AddItemToInventory(Item item)
    //{
    //    if (inventoryItemPrefab == null)
    //    {
    //        Debug.LogError("inventoryItemPrefab is NOT assigned!");
    //        return;
    //    }

    //    GameObject itemObject = Instantiate(inventoryItemPrefab);
    //    InventoryItem invItem = itemObject.GetComponent<InventoryItem>();
    //    if (invItem == null)
    //    {
    //        Debug.LogError("InventoryItem component missing on prefab!");
    //        Destroy(itemObject);
    //        return;
    //    }

    //    invItem.InitialiseItem(item);

    //    foreach (var slot in Object.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None))
    //    {
    //        if (slot.transform.childCount == 0)
    //        {
    //            itemObject.transform.SetParent(slot.transform, false);
    //            return;
    //        }
    //    }

    //    Destroy(itemObject); // No free slot
    //    Debug.LogWarning("Inventory is full!");
    //}
    private void AddItemToInventory(Item item)
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is not assigned!");
            return;
        }

        // Try to add the item using the InventoryManager
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
