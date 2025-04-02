using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public WeaponItem equippedWeapon;
    public Transform firePoint;

    public void Equip(Item item)
    {
        Debug.Log("Trying to equip: " + item.itemName + " (" + item.type + ")");
        if (item is WeaponItem weapon)
        {
            equippedWeapon = weapon;
            Debug.Log($"Equipped weapon: {weapon.itemName}");
        }

        // You can add armor/other types here later too
    }
    public void Unequip(Item item)
    {
        if (item is WeaponItem && equippedWeapon == item)
        {
            equippedWeapon = null;
            Debug.Log("Weapon unequipped.");
        }

        // You can add armor/other types here later too
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