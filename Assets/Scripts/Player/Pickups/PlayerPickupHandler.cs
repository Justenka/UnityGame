using UnityEngine;

public class PlayerPickupHandler : MonoBehaviour
{
    public Player player;
    public InventoryManager inventory;

    public void HandlePickup(PickupItem pickup)
    {
        switch (pickup.type)
        {
            case PickupType.Currency:
                player.GetCurrency(pickup.amount);
                break;

            case PickupType.Item:
                if (pickup.itemData is Item item)
                {
                    for (int i = 0; i < pickup.amount; i++)
                    {
                        inventory.AddItem(item, 1);
                    }
                    Debug.Log($"Picked up {pickup.amount}x {item.name}");
                }
                else
                {
                    Debug.LogWarning("Invalid itemData assigned to PickupItem.");
                }
                break;

            default:
                Debug.LogWarning("Unknown pickup type.");
                break;
        }
    }
}
