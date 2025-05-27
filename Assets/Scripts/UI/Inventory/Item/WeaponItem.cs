using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : EquipmentItem
{
    public float fireRate;
    public ActionType actionType;
    public GameObject projectilePrefab; // For ranged weapons
    public float projectileSpeed = 10f; // Optional
    public float manaCost = 10f;        // Optional

    public Sprite weaponSprite;
    public RuntimeAnimatorController animatorController;
    public float xOffset = 0.01f;
    public float yOffset = 0.15f;
    public DebuffData debuffData;
    public override bool Use(GameObject user)
    {
        return true;
    }
}