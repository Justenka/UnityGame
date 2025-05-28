using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI rarity;
    public TextMeshProUGUI stats;
    public RectTransform backgroundRect;

    [Header("Enchant Info")]
    public GameObject enchantUI;
    public TextMeshProUGUI enchantName;
    public TextMeshProUGUI enchantDuration;
    public TextMeshProUGUI enchantDamage;

    public Vector2 offset = new Vector2(20, -20);
    public Vector2 padding = new Vector2(10, 10);

    void Update()
    {
        Vector2 position = (Vector2)Input.mousePosition + offset;

        float width = backgroundRect.sizeDelta.x;
        float height = backgroundRect.sizeDelta.y;

        Vector2 size = backgroundRect.rect.size;
        Vector2 scaledSize = new Vector2(size.x * backgroundRect.lossyScale.x, size.y * backgroundRect.lossyScale.y);

        position.x = Mathf.Clamp(position.x, 0 + padding.x, Screen.width - scaledSize.x - padding.x);
        position.y = Mathf.Clamp(position.y, scaledSize.y + padding.y, Screen.height - padding.y);

        transform.position = position;
    }

    public void Show(Item item)
    {
        if (item == null) return;

        if (item is EquipmentItem eqItem && eqItem.equipmentLevel > 0)
        {
            itemName.text = $"{eqItem.itemName} +{eqItem.equipmentLevel}";
        }
        else
        {
            itemName.text = item.itemName;
        }

        if (item is WeaponItem weapon && weapon.debuffData.debuffType != DebuffType.None)
        {
            enchantUI.SetActive(true);
            string debuffColor = GetColorForDebuff(weapon.debuffData.debuffType);
            enchantName.text = $"<color={debuffColor}>{weapon.debuffData.debuffType}</color>";
            enchantDuration.text = $"Duration: {weapon.debuffData.duration}";
            enchantDamage.text = $"Damage: {weapon.debuffData.damagePerTick}";
        }
        else
        {
            enchantUI.SetActive(false);
        }

        string rarityColor = GetColorForRarity(item.rarity);
       //rarity.text = item.rarity.ToString();
        rarity.text = $"<color={rarityColor}>{item.rarity.ToString()}</color>\n";
        if (item.statModifiers != null && item.statModifiers.Count > 0)
        {
            stats.text = "";
            foreach (var mod in item.statModifiers)
            {
                string statColor = GetColorForStat(mod.statType);
                string sign = mod.value >= 0 ? "+" : "-";
                stats.text += $"<color={statColor}>{sign}{Mathf.Abs(mod.value)} {mod.statType}</color>\n";
            }

            description.text = item.description;
        }
        else
        {
            stats.text = item.description;
            description.text = "";
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        enchantUI.SetActive(false);
    }

    string GetColorForStat(StatType stat)
    {
        return stat switch
        {
            StatType.Health => "#ff4444",
            StatType.Mana => "#4444ff",
            StatType.Stamina => "#44ff44",
            StatType.Attack => "#ffcc00",
            StatType.Defense => "#00ffee",
            StatType.Speed => "#ff77ff",
            StatType.CritChance => "#ff88ff",
            _ => "#ffffff"
        };
    }
    string GetColorForRarity(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common => "#ffffff",
            ItemRarity.Uncommon => "#1EFF00",
            ItemRarity.Rare => "#0070FF",
            ItemRarity.Epic => "#A335EE",
            ItemRarity.Legendary => "#FF8000",
            _ => "#ffffff"
        };
    }
    string GetColorForDebuff(DebuffType debuff)
    {
        return debuff switch
        {
            DebuffType.Poison => "#44ff44", // green
            DebuffType.Burn => "#ff4444",   // red
            DebuffType.Slow => "#44ccff",   // blue
            DebuffType.Stun => "#ffcc00",   // yellow
            _ => "#ffffff"
        };
    }
}
