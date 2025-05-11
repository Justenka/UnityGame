using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Player player;
    public ShopItemSlot[] shopSlots;
    public Item[] itemsForSale;
    public GameObject inventoryItemPrefab;
    public InventoryManager inventoryManager;
    PlayerAudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
    }

    protected virtual void OnEnable()
    {
        PopulateShop();
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
            audioManager.PlaySound(audioManager.money);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    private void AddItemToInventory(Item item)
    {
        if (inventoryManager.AddItem(item, 1))
        {
            Debug.Log($"Item {item.itemName} added to inventory.");
        }
        else
        {
            Debug.LogWarning("Inventory is full, could not add item.");
        }
    }
}
