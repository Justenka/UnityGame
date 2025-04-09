using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Player player;
    private WeaponRotation weaponRotation;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        
        // This assumes WeaponParent is a sibling of AttackArea
        Transform weaponParentTransform = transform.parent.Find("WeaponParent");
        if (weaponParentTransform != null)
        {
            weaponRotation = weaponParentTransform.GetComponent<WeaponRotation>();
        }

        if (weaponRotation == null)
        {
            Debug.LogError("WeaponRotation is NULL! Check if WeaponParent is named correctly.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            WeaponItem weapon = player.GetComponent<EquipmentManager>().equippedWeapon;

            if (weaponRotation.Attack())
            {
                if (weapon != null)
                {
                    float damage = player.stats[StatType.Attack].Total;
                    float knockBack = 5f;
                    bool doesKnockBack = true;
                    enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
                }
            }
        }
    }
}