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
    public TextMeshProUGUI successChanceText;
    public TextMeshProUGUI loseLevelChanceText;
    public TextMeshProUGUI destroyChanceText;
    public Button upgradeButton;

    [Header("Risk Upgrade Settings")]
    public Toggle riskToggle; // Toggle in UI to enable risk mode

    private void OnEnable()
    {
        UpdateCostDisplay();

        if (riskToggle != null)
            riskToggle.onValueChanged.AddListener(OnRiskToggleChanged);
    }

    private void OnDisable()
    {
        if (riskToggle != null)
            riskToggle.onValueChanged.RemoveListener(OnRiskToggleChanged);
    }

    private void OnRiskToggleChanged(bool isOn)
    {
        UpdateCostDisplay();
    }

    public void UpdateCostDisplay()
    {
        var eq = equipmentSlot.GetCurrentItem() as EquipmentItem;

        if (eq == null)
        {
            costText.text = "Cost: -";
            upgradeButton.interactable = false;
            SetRiskTexts("-", "-", "-");
            return;
        }

        int goldCost = 10 + eq.equipmentLevel * 10;
        costText.text = $"Cost: {goldCost}g";

        bool canAfford = player.currencyHeld >= goldCost;
        costText.color = canAfford ? Color.white : Color.red;
        upgradeButton.interactable = canAfford;

        if (riskToggle != null && riskToggle.isOn)
        {
            var (success, drop, destroy) = GetScaledChances(eq.equipmentLevel);
            SetRiskTexts($"{success}%", $"{drop}%", $"{destroy}%");
        }
        else
        {
            SetRiskTexts("-", "-", "-");
        }
    }

    private void SetRiskTexts(string success, string drop, string destroy)
    {
        if (successChanceText != null) successChanceText.text = $"Success: {success}";
        if (loseLevelChanceText != null) loseLevelChanceText.text = $"Lose Level: {drop}";
        if (destroyChanceText != null) destroyChanceText.text = $"Destroy: {destroy}";
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

        if (eq.equipmentLevel >= maxAllowedLevel) return;

        int goldCost = 10 + eq.equipmentLevel * 10;
        if (player.currencyHeld < goldCost) return;

        player.currencyHeld -= goldCost;

        bool isRiskUpgrade = riskToggle != null && riskToggle.isOn;

        if (isRiskUpgrade)
        {
            var (successChance, levelDropChance, destructionChance) = GetScaledChances(eq.equipmentLevel);
            int roll = Random.Range(0, 100);

            if (roll < successChance)
            {
                int newLevel = eq.equipmentLevel + 2;
                eq.ApplyUpgrade(Mathf.Min(newLevel, maxAllowedLevel)); // Successful risk upgrade
            }
            else
            {
                int failRoll = Random.Range(0, 100);

                if (failRoll < destructionChance)
                {
                    equipmentSlot.ClearSlot();
                }
                else if (failRoll < destructionChance + levelDropChance)
                {
                    eq.ApplyUpgrade(Mathf.Max(0, eq.equipmentLevel - 1)); // Lose 1 level
                }
                // Else: fail but no penalty
            }
        }
        else
        {
            eq.ApplyUpgrade(Mathf.Min(eq.equipmentLevel + 1, maxAllowedLevel)); // Normal upgrade
        }

        // Consume material
        materialItemObj.count--;
        if (materialItemObj.count <= 0)
            Destroy(materialItemObj.gameObject);
        else
            materialItemObj.RefreshCount();

        UpdateCostDisplay();
    }

    private (int success, int drop, int destroy) GetScaledChances(int level)
    {
        switch (level)
        {
            case 0: return (70, 20, 10);
            case 1: return (65, 20, 15);
            case 2: return (60, 25, 15);
            case 3: return (55, 25, 20);
            case 4: return (50, 30, 20);
            case 5: return (45, 30, 25);
            case 6: return (40, 35, 25);
            case 7: return (38, 35, 27);
            case 8: return (36, 36, 28);
            case 9: return (34, 36, 30);
            case 10: return (32, 38, 30);
            case 11: return (30, 40, 30);
            case 12: return (28, 40, 32);
            case 13: return (26, 42, 32);
            case 14: return (24, 43, 33);
            case 15: return (22, 44, 34);
            case 16: return (20, 45, 35);
            case 17: return (18, 46, 36);
            case 18: return (16, 47, 37);
            case 19: return (14, 48, 38);
            case 20: return (10, 50, 40);
            default: return (5, 50, 45); // For over-cap or invalid levels
        }
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
        if (itemObj == null) return;

        bool added = inventoryManager.AddItem(itemObj.item, itemObj.count);
        if (added)
            Destroy(itemObj.gameObject);
    }
}
