using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableItem : Item
{
    public ConsumableType consumableType;
    public int restoreAmount;
    public float cooldown;

    public override void Use(GameObject user)
    {
        Player player = user.GetComponent<Player>();
        if (player == null) return;

        switch (consumableType)
        {
            case ConsumableType.Health:
                player.RestoreHealth(restoreAmount); 
                break;

            case ConsumableType.Stamina:
                player.RestoreStamina(restoreAmount);
                break;

            case ConsumableType.Mana:
                player.RestoreMana(restoreAmount);
                break;
        }
    }
}