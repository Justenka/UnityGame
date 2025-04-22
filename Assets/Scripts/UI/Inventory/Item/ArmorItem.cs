using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorItem : EquipmentItem
{
    public override bool Use(GameObject user)
    {
        return true; // Could apply a buff, trigger animation, etc.
    }
}