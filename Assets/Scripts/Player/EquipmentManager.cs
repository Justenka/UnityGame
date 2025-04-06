using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public WeaponItem equippedWeapon;
    public Transform firePoint;
    public GameObject weaponVisual;

    void Start()
    {
        // If weapon is equipped, show the visual; otherwise hide it
        if (weaponVisual != null)
            weaponVisual.SetActive(equippedWeapon != null);
    }
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

            // Show weapon visual
            if (weaponVisual != null)
                weaponVisual.SetActive(true);
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

            // Hide weapon visual
            if (weaponVisual != null)
                weaponVisual.SetActive(false);
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