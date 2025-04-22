using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public float fireRate;
    public ActionType actionType;
    public GameObject projectilePrefab; // For ranged weapons
    public float projectileSpeed = 10f; // Optional
    public float manaCost = 10f;        // Optional

    public Sprite weaponSprite;
    public float xOffset = 0.01f;
    public float yOffset = 0.15f;

    public DebuffData debuffData;
    public override bool Use(GameObject user)
    {
        // Do attack logic
        return true;
    }
}