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
                ApplyDebuffToEnemy(enemy, weapon);
                enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
            }
        }
    }
    void ApplyDebuffToEnemy(Enemy enemy, WeaponItem weapon)
    {
        if (enemy == null) return;
        switch (weapon.debuffData.debuffType)
        {
            case DebuffType.Poison:
                enemy.AddDebuff(new PoisonDebuff(
                    weapon.debuffData.duration,
                    weapon.debuffData.damagePerTick,
                    weapon.debuffData.tickInterval));
                break;
            case DebuffType.Burn:
                enemy.AddDebuff(new BurnDebuff(
                    weapon.debuffData.duration,
                    weapon.debuffData.damagePerTick,
                    weapon.debuffData.tickInterval));
                break;
            case DebuffType.Slow:
                enemy.AddDebuff(new SlowDebuff(
                    weapon.debuffData.duration,
                    weapon.debuffData.slowAmount));
                break;
        }
    }
}