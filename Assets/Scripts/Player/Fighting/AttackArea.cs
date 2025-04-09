using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Player player;
    private bool isAttacking = false;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isAttacking) return;

        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            WeaponItem weapon = player.GetComponent<EquipmentManager>().equippedWeapon;
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