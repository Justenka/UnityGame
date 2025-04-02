using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorItem : Item
{
    public int defense;

    public override void Use(GameObject user)
    {
        // Equip armor logic
    }
}