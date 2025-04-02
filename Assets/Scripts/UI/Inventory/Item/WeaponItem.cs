using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public int damage;
    public float attackSpeed;
    public ActionType actionType;
    public GameObject projectilePrefab; // For ranged weapons
    public float manaCost = 10f;        // Optional

    public override void Use(GameObject user)
    {
        // Do attack logic
    }
}