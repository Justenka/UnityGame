using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("Slots")]
    public InventorySlot equipmentSlot;
    public InventorySlot materialSlot;

    [Header("References")]
    public Player player;
    public InventoryManager inventoryManager;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    private void OnEnable() => UpdateCostDisplay();

    public void UpdateCostDisplay()
    {
        var eq = equipmentSlot.GetCurrentItem() as EquipmentItem;

        if (eq == null)
        {
            costText.text = "Cost: -";
            costText.color = Color.white;
            upgradeButton.interactable = false;
            return;
        }

        int goldCost = 10 + eq.equipmentLevel * 10;
        costText.text = $"Cost: {goldCost} gold";
        bool canAfford = player.currencyHeld >= goldCost;
        costText.color = canAfford ? Color.white : Color.red;
        upgradeButton.interactable = canAfford;
    }

    public void TryUpgrade()
    {
        var eq = equipmentSlot.GetCurrentItem() as EquipmentItem;
        var materialItemObj = materialSlot.GetComponentInChildren<InventoryItem>();
        var mat = materialItemObj?.item as MaterialItem;

        if (eq == null || mat == null) return;

        int maxAllowedLevel = mat.tier switch
        {
            1 => 5,
            2 => 10,
            3 => 15,
            4 => 20,
            _ => 0
        };

        if (eq.equipmentLevel >= maxAllowedLevel)
        {
            return;
        }

        int goldCost = 10 + eq.equipmentLevel * 10;

        if (player.currencyHeld < goldCost)
        {
            return;
        }

        player.currencyHeld -= goldCost;
        eq.ApplyUpgrade(eq.equipmentLevel + 1);

        materialItemObj.count--;
        if (materialItemObj.count <= 0)
            Destroy(materialItemObj.gameObject);
        else
            materialItemObj.RefreshCount();

        UpdateCostDisplay();
    }

    public void ReturnItemsToInventory()
    {
        ReturnItem(equipmentSlot);
        ReturnItem(materialSlot);
        UpdateCostDisplay();
    }

    private void ReturnItem(InventorySlot slot)
    {
        var itemObj = slot.GetComponentInChildren<InventoryItem>();
        if (itemObj == null)
        {
            return;
        }
        bool added = inventoryManager.AddItem(itemObj.item, itemObj.count);
        if (added)
            Destroy(itemObj.gameObject);

    }
}
