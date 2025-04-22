using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableItem : Item
{
    public ConsumableType consumableType;
    public int restoreAmount;
    public float cooldown;

    public override bool Use(GameObject user)
    {
        Player player = user.GetComponent<Player>();
        if (player == null) return false;

        switch (consumableType)
        {
            case ConsumableType.Health:
                return player.RestoreHealth(restoreAmount);

            case ConsumableType.Stamina:
                return player.RestoreStamina(restoreAmount);

            case ConsumableType.Mana:
                return player.RestoreMana(restoreAmount);

            case ConsumableType.Unknown:
                UnknownConsumableEffects.ApplyRandomEffect(player);
                return true; // assume something always happens
        }

        return false;
    }
}
