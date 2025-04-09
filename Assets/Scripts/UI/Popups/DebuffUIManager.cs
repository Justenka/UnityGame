using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DebuffUIManager : MonoBehaviour
{
    public Image poisonIcon;
    public Image burnIcon;
    public Image slowIcon;

    public TMP_Text poisonTimeText;
    public TMP_Text burnTimeText;
    public TMP_Text slowTimeText;

    private Dictionary<DebuffType, Image> debuffIcons = new Dictionary<DebuffType, Image>();
    private Dictionary<DebuffType, TMP_Text> debuffTimeTexts = new Dictionary<DebuffType, TMP_Text>();

    void Start()
    {
        // Populate the debuff icon and time text dictionaries.
        debuffIcons[DebuffType.Poison] = poisonIcon;
        debuffIcons[DebuffType.Burn] = burnIcon;
        debuffIcons[DebuffType.Slow] = slowIcon;

        debuffTimeTexts[DebuffType.Poison] = poisonTimeText;
        debuffTimeTexts[DebuffType.Burn] = burnTimeText;
        debuffTimeTexts[DebuffType.Slow] = slowTimeText;

        // Initially, hide all icons and texts.
        HideAllIcons();
    }

    public void ShowDebuffIcon(DebuffType debuffType)
    {
        if (debuffIcons.ContainsKey(debuffType))
        {
            debuffIcons[debuffType].enabled = true;
            debuffTimeTexts[debuffType].enabled = true;
        }
    }

    public void HideDebuffIcon(DebuffType debuffType)
    {
        if (debuffIcons.ContainsKey(debuffType))
        {
            debuffIcons[debuffType].enabled = false;
            debuffTimeTexts[debuffType].enabled = false;
        }
    }

    public void HideAllIcons()
    {
        foreach (var icon in debuffIcons.Values)
        {
            icon.enabled = false;
        }
        foreach (var text in debuffTimeTexts.Values)
        {
            text.enabled = false;
        }
    }

    public void UpdateDebuffTime(DebuffType debuffType, float remainingTime)
    {
        if (debuffTimeTexts.ContainsKey(debuffType))
        {
            debuffTimeTexts[debuffType].text = Mathf.Ceil(remainingTime).ToString();
        }
    }
}
