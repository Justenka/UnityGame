using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            WeaponItem weapon = player.GetComponent<EquipmentManager>().equippedWeapon;

            if (weapon != null)
            {
                float knockBack = 5f; // You can also store this in WeaponItem
                bool doesKnockBack = true;
                enemy.TakeDamage(weapon.damage, transform.position, knockBack, doesKnockBack);
            }
        }
    }
}