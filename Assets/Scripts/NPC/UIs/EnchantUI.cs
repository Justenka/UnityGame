using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EnchantUI : MonoBehaviour
{
    [Header("Slots")]
    public InventorySlot weaponSlot;

    [Header("UI Elements")]
    public Transform debuffListContainer;
    public GameObject debuffTogglePrefab;
    public TextMeshProUGUI costText;
    public Button enchantButton;

    [Header("References")]
    public Player player;
    public InventoryManager inventoryManager;

    private DebuffType selectedDebuff = DebuffType.None;
    private int enchantCost = 50;

    private void OnEnable()
    {
        GenerateDebuffList();
        UpdateCostDisplay();
    }

    private void GenerateDebuffList()
    {
        foreach (Transform child in debuffListContainer)
            Destroy(child.gameObject);

        foreach (DebuffType debuff in System.Enum.GetValues(typeof(DebuffType)))
        {
            if (debuff == DebuffType.None) continue;

            var toggleObj = Instantiate(debuffTogglePrefab, debuffListContainer);
            var toggle = toggleObj.GetComponent<Toggle>();
            var label = toggleObj.GetComponentInChildren<TMP_Text>();
            label.text = debuff.ToString();

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    selectedDebuff = debuff;
                    UpdateCostDisplay();
                }
            });
        }
    }

    public void UpdateCostDisplay()
    {
        var weapon = weaponSlot.GetCurrentItem() as WeaponItem;

        if (weapon == null || selectedDebuff == DebuffType.None)
        {
            costText.text = "Cost: -";
            costText.color = Color.white;
            enchantButton.interactable = false;
            return;
        }

        bool canAfford = player.currencyHeld >= enchantCost;
        costText.text = $"Cost: {enchantCost} gold";
        costText.color = canAfford ? Color.white : Color.red;
        enchantButton.interactable = canAfford;
    }

    public void TryEnchant()
    {
        var weapon = weaponSlot.GetCurrentItem() as WeaponItem;
        if (weapon == null || selectedDebuff == DebuffType.None) return;

        if (player.currencyHeld < enchantCost) return;

        player.currencyHeld -= enchantCost;

        // Apply the new enchant (override)
        weapon.debuffData.debuffType = selectedDebuff;
        Debug.Log($"Enchanted {weapon.name} with {selectedDebuff}");

        UpdateCostDisplay();
    }

    public void ReturnWeaponToInventory()
    {
        ReturnItem(weaponSlot);
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
