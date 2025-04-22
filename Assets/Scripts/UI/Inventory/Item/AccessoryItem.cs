using UnityEngine;

[CreateAssetMenu(menuName = "Items/Accessory")]
public class AccessoryItem : EquipmentItem
{
    public override bool Use(GameObject user)
    {
        return true; // Could apply a buff, trigger animation, etc.
    }
}