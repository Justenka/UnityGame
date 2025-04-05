using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public float fireRate;
    public ActionType actionType;
    public GameObject projectilePrefab; // For ranged weapons
    public float projectileSpeed = 10f; // Optional
    public float manaCost = 10f;        // Optional

    public override void Use(GameObject user)
    {
        // Do attack logic
    }
}