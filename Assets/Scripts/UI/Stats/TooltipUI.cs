using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI stats;
    public RectTransform backgroundRect;

    public Vector2 offset = new Vector2(20, -20);
    public Vector2 padding = new Vector2(10, 10);

    void Update()
    {
        Vector2 position = (Vector2)Input.mousePosition + offset;

        float width = backgroundRect.sizeDelta.x;
        float height = backgroundRect.sizeDelta.y;

        Vector2 size = backgroundRect.rect.size;
        Vector2 scaledSize = new Vector2(size.x * backgroundRect.lossyScale.x, size.y * backgroundRect.lossyScale.y);

        // Clamp to keep entire tooltip within screen bounds
        position.x = Mathf.Clamp(position.x, 0 + padding.x, Screen.width - scaledSize.x - padding.x);
        position.y = Mathf.Clamp(position.y, scaledSize.y + padding.y, Screen.height - padding.y);

        transform.position = position;
    }

    public void Show(Item item)
    {
        if (item == null) return;

        itemName.text = item.itemName;
        description.text = item.description;

        stats.text = "";
        if (item.statModifiers != null && item.statModifiers.Count > 0)
        {
            foreach (var mod in item.statModifiers)
            {
                string statColor = GetColorForStat(mod.statType);
                string sign = mod.value >= 0 ? "+" : "-";
                stats.text += $"<color={statColor}>{sign}{Mathf.Abs(mod.value)} {mod.statType}</color>\n";
            }
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
}
