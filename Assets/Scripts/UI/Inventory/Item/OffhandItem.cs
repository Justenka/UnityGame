using UnityEngine;

[CreateAssetMenu(fileName = "New Offhand", menuName = "Items/Offhand")]
public class OffhandItem : EquipmentItem
{
    public Sprite shieldSprite;  // The sprite to display when equipped
    public float defenseBonus;   // Optional: Adds to player's defense
    public float xOffset = 0f;   // Adjust shield position relative to player
    public float yOffset = 0f;

    public override bool Use(GameObject user)
    {
        // Optional: Add logic for shield abilities (e.g., blocking)
        Debug.Log("Using shield: " + itemName);
        return true;
    }
}