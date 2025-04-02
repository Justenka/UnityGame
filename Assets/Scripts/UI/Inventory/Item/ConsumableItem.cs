using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableItem : Item
{
    public int healAmount;

    public override void Use(GameObject user)
    {
        // Heal user
    }
}