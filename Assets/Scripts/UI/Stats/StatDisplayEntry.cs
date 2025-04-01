using TMPro;
using UnityEngine;

public class StatDisplayEntry : MonoBehaviour
{
    public TMP_Text statNameText;
    public TMP_Text statValueText;

    private StatType statType;

    public void SetStat(StatType stat, float value)
    {
        statType = stat;
        statNameText.text = stat.ToString();
        statValueText.text = value.ToString("F0");
    }

    public void UpdateValue(float value)
    {
        statValueText.text = value.ToString("F0");
    }
}
