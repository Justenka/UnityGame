using UnityEngine;

public class PlayerPickupHandler : MonoBehaviour
{
    public Player player;
    // public PlayerInventory inventory; // For items later

    public void HandlePickup(PickupItem pickup)
    {
        switch (pickup.type)
        {
            case PickupType.Currency:
                player.GetCurrency(pickup.amount);
                break;

            case PickupType.Item:
                Debug.Log("Picked up item: " + pickup.itemData.name);
                // inventory.AddItem((ItemSO)pickup.itemData);
                break;

            case PickupType.Potion:
                Debug.Log("Picked up potion: +" + pickup.amount);
                // Add health, mana, etc.
                break;

            default:
                Debug.LogWarning("Unknown pickup type.");
                break;
        }
    }
}