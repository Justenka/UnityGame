using UnityEngine;

public abstract class EquipmentItem : Item
{
    public EquipmentType equipmentType;
    public int equipmentLevel;

    public void ApplyUpgrade(int newLevel)
    {
        equipmentLevel = newLevel;

        foreach (var mod in statModifiers)
        {
            mod.value = Mathf.Round(mod.baseValue * (1f + 0.02f * newLevel));
        }
    }
}