using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public WeaponItem equippedWeapon;
    public Transform firePoint;
    public GameObject weaponVisual;

    public OffhandItem equippedShield;
    public GameObject shieldVisual;
    private SpriteRenderer shieldSpriteRenderer;

    // Use a HashSet to prevent duplicates
    private HashSet<Item> equippedItems = new HashSet<Item>();
    private SpriteRenderer weaponSpriteRenderer;
    private Transform weaponSpriteTransform;

    //public Animator playerAnimator;
    public RuntimeAnimatorController swordAnimatorController;
    public RuntimeAnimatorController staffAnimatorController;
    PlayerAudioManager audioManager;

    public static event Action OnWeaponEquipped;
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
        if (!equippedItems.Contains(item))
        {
            audioManager.PlaySound(audioManager.gearing);
        }

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
                    if (weapon.animatorController != null)
                    {
                        weaponAnimator.runtimeAnimatorController = weapon.animatorController;
                        Debug.Log($"Animator set to: {weapon.animatorController.name} for weapon {weapon.itemName}");
                    }
                    else
                    {
                        Debug.LogWarning($"No animator controller assigned for weapon: {weapon.itemName}");
                    }
                }
                else
                {
                    Debug.LogError("Animator not found on weaponVisual.");
                }
            }

            audioManager.PlaySound(audioManager.gearing);
            Debug.Log("Weapon equipped: " + weapon.itemName);
            OnWeaponEquipped?.Invoke();
        }
        else if (item is OffhandItem shield)
        {
            equippedShield = shield;
            if (shieldVisual != null)
            {
                shieldVisual.SetActive(true);
                shieldSpriteRenderer = shieldVisual.GetComponent<SpriteRenderer>();
                if (shieldSpriteRenderer != null && shield.shieldSprite != null)
                {
                    shieldSpriteRenderer.sprite = shield.shieldSprite;
                }
            }
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
        else if (item is OffhandItem && equippedShield == item)
        {
            equippedShield = null;
            if (shieldVisual != null)
                shieldVisual.SetActive(false);
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