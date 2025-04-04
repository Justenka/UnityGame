using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableItem : Item
{
    public ConsumableType consumableType;
    public int healAmount;
    public float cooldown;

    public override void Use(GameObject user)
    {
        Player player = user.GetComponent<Player>();
        if (player != null)
        {
            player.UseHealth(-healAmount); // healing by negative damage
            Debug.Log($"Player healed by {healAmount} HP.");
        }
    }
}