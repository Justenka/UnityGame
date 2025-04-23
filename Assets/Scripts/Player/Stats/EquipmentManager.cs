using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public WeaponItem equippedWeapon;
    public Transform firePoint;
    public GameObject weaponVisual;

    // Use a HashSet to prevent duplicates
    private HashSet<Item> equippedItems = new HashSet<Item>();
    private SpriteRenderer weaponSpriteRenderer;
    private Transform weaponSpriteTransform;
    //public Animator playerAnimator;
    public RuntimeAnimatorController swordAnimatorController;
    public RuntimeAnimatorController staffAnimatorController;
    PlayerAudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
        if (weaponVisual != null)
        {
            weaponVisual.SetActive(equippedWeapon != null);
            weaponSpriteRenderer = weaponVisual.GetComponent<SpriteRenderer>();
            weaponSpriteTransform = weaponVisual.transform.Find("Weapon");

            if (weaponSpriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found on weaponVisual GameObject!");
            }
        }
        else
        {
            Debug.LogError("weaponVisual not assigned in EquipmentManager!");
        }
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
            {
                weaponVisual.SetActive(true);

                if (weaponSpriteRenderer == null)
                    weaponSpriteRenderer = weaponVisual.GetComponent<SpriteRenderer>();

                if (weaponSpriteRenderer != null)
                {
                    if (weapon.weaponSprite != null)
                    {
                        Debug.Log("Setting sprite to: " + weapon.weaponSprite.name);
                        weaponSpriteRenderer.sprite = weapon.weaponSprite;
                    }
                    else
                    {
                        Debug.LogWarning("No sprite assigned to weapon: " + weapon.itemName);
                    }
                }
                else
                {
                    Debug.LogError("weaponSpriteRenderer is NULL. Check Weapon GameObject.");
                }

                Animator weaponAnimator = weaponVisual.GetComponent<Animator>();
                if (weaponAnimator != null)
                {
                    if (weapon.itemName == "Sword" && swordAnimatorController != null)
                    {
                        weaponAnimator.runtimeAnimatorController = swordAnimatorController;
                        Debug.Log("Animator set to Sword");
                    }
                    else if (weapon.itemName == "Staff" && staffAnimatorController != null)
                    {
                        weaponAnimator.runtimeAnimatorController = staffAnimatorController;
                        Debug.Log("Animator set to Staff");
                    }
                    else
                    {
                        Debug.LogWarning($"No animator controller found for {weapon.itemName}");
                    }
                }
                else
                {
                    Debug.LogError("Animator not found on weaponVisual.");
                }
            }
            audioManager.PlaySound(audioManager.gearing);
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
        audioManager.PlaySound(audioManager.gearing);
        Debug.Log("Unequipping: " + item.itemName);

        Player player = GetComponent<Player>();
        if (player != null)
            player.RemoveItemStats(item);

        if (item is WeaponItem && equippedWeapon == item)
        {
            equippedWeapon = null;

            if (weaponVisual != null)
            {
                weaponVisual.SetActive(false);

                if (weaponSpriteRenderer != null)
                    weaponSpriteRenderer.sprite = null;
            }
            audioManager.PlaySound(audioManager.gearing);
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
        audioManager.PlaySound(audioManager.gearing);
        equippedItems.Clear();
    }

    public void UseWeapon(GameObject user)
    {
        if (equippedWeapon == null || UIManager.Instance.openMenus.Count > 0) return;
        switch (equippedWeapon.actionType)
        {
            case ActionType.Melee:
                user.GetComponent<PlayerAttack>().DoAttack();
                audioManager.PlaySound(audioManager.swoosh);
                break;

            case ActionType.Ranged:
                user.GetComponent<PlayerShooting>().DoShoot(equippedWeapon);
                audioManager.PlaySound(audioManager.pop);
                break;
        }
    }

    public bool IsEquipped(Item item)
    {
        return equippedItems.Contains(item);
    }

    public SpriteRenderer GetWeaponSpriteRenderer()
    {
        if (weaponSpriteRenderer == null && weaponVisual != null)
        {
            weaponSpriteRenderer = weaponVisual.GetComponent<SpriteRenderer>();
        }

        if (weaponSpriteRenderer == null)
        {
            Debug.LogError("Failed to get weapon SpriteRenderer.");
        }

        return weaponSpriteRenderer;
    }
}