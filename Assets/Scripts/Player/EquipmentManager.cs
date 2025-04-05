using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public WeaponItem equippedWeapon;
    public Transform firePoint;

    public void Equip(Item item)
    {
        Debug.Log("Trying to equip: " + item.itemName + " (" + item.type + ")");

        Player player = GetComponent<Player>();
        if (player != null)
            player.ApplyItemStats(item);

        if (item is WeaponItem weapon)
        {
            equippedWeapon = weapon;
            Debug.Log($"Equipped weapon: {weapon.itemName}");
        }
    }
    public void Unequip(Item item)
    {
        Player player = GetComponent<Player>();
        if (player != null)
            player.RemoveItemStats(item);

        if (item is WeaponItem && equippedWeapon == item)
        {
            equippedWeapon = null;
            Debug.Log("Weapon unequipped.");
        }
    }

    public void UseWeapon(GameObject user)
    {
        if (equippedWeapon == null) return;

        switch (equippedWeapon.actionType)
        {
            case ActionType.Melee:
                user.GetComponent<PlayerAttack>().DoAttack();
                break;

            case ActionType.Ranged:
                user.GetComponent<PlayerShooting>().DoShoot(equippedWeapon);
                break;
        }
    }
}