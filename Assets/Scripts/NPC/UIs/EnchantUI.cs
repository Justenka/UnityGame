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
    public int enchantCost;
    private List<Toggle> debuffToggles = new();

    private void OnEnable()
    {
        GenerateDebuffList();
        UpdateCostDisplay();
    }

    private void GenerateDebuffList()
    {
        debuffToggles.Clear();

        foreach (Transform child in debuffListContainer)
            Destroy(child.gameObject);

        foreach (DebuffType debuff in System.Enum.GetValues(typeof(DebuffType)))
        {
            if (debuff == DebuffType.None) continue;

            var toggleObj = Instantiate(debuffTogglePrefab, debuffListContainer);
            var toggle = toggleObj.GetComponent<Toggle>();
            var label = toggleObj.GetComponentInChildren<TMP_Text>();
            label.text = debuff.ToString();
            debuffToggles.Add(toggle);

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    // Uncheck all other toggles
                    foreach (var t in debuffToggles)
                    {
                        if (t != toggle) t.isOn = false;
                    }

                    selectedDebuff = debuff;
                }
                else if (selectedDebuff == debuff)
                {
                    selectedDebuff = DebuffType.None;
                }

                UpdateCostDisplay();
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

        bool isSameDebuff = weapon.debuffData.debuffType == selectedDebuff;
        bool canAfford = player.currencyHeld >= enchantCost;

        costText.text = isSameDebuff
            ? "Already Enchanted"
            : $"Cost: {enchantCost}g";

        costText.color = (canAfford && !isSameDebuff) ? Color.white : Color.red;
        enchantButton.interactable = canAfford && !isSameDebuff;
    }

    public void TryEnchant()
    {
        var weapon = weaponSlot.GetCurrentItem() as WeaponItem;
        if (weapon == null || selectedDebuff == DebuffType.None) return;

        if (player.currencyHeld < enchantCost) return;

        if (weapon.debuffData.debuffType == selectedDebuff)
        {
            Debug.Log("Weapon already has this enchantment.");
            return;
        }

        player.RemoveCurrency(enchantCost);
        weapon.debuffData.debuffType = selectedDebuff;
        weapon.debuffData.damagePerTick = 10;
        weapon.debuffData.duration = 3;
        weapon.debuffData.tickInterval = 2;
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
