using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public WeaponItem equippedWeapon;
    public Transform firePoint;
    public GameObject weaponVisual;

    // Use a HashSet to prevent duplicates
    private HashSet<Item> equippedItems = new HashSet<Item>();

    void Start()
    {
        if (weaponVisual != null)
            weaponVisual.SetActive(equippedWeapon != null);
    }

    public void Equip(Item item)
    {
        if (equippedItems.Contains(item))
        {
            Debug.LogWarning("Item already equipped: " + item.itemName);
            return;
        }

        Debug.Log("Equipping: " + item.itemName);

        Player player = GetComponent<Player>();
        if (player != null)
            player.ApplyItemStats(item);

        if (item is WeaponItem weapon)
        {
            equippedWeapon = weapon;
            if (weaponVisual != null)
                weaponVisual.SetActive(true);
            Debug.Log("Weapon equipped: " + weapon.itemName);
        }

        equippedItems.Add(item);
    }

    public void Unequip(Item item)
    {
        if (!equippedItems.Contains(item))
        {
            Debug.LogWarning("Tried to unequip item not equipped: " + item.itemName);
            return;
        }

        Debug.Log("Unequipping: " + item.itemName);

        Player player = GetComponent<Player>();
        if (player != null)
            player.RemoveItemStats(item);

        if (item is WeaponItem && equippedWeapon == item)
        {
            equippedWeapon = null;
            if (weaponVisual != null)
                weaponVisual.SetActive(false);
            Debug.Log("Weapon unequipped.");
        }

        equippedItems.Remove(item);
    }

    public void UnequipAll()
    {
        foreach (Item item in new List<Item>(equippedItems))
        {
            Unequip(item);
        }

        equippedItems.Clear();
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
    public bool IsEquipped(Item item)
    {
        return equippedItems.Contains(item);
    }
}
