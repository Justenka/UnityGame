using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    public float lifetime = 10f;
    public float knockBack = 1.5f;
    public bool doesKnockBack = true;
    public DebuffData debuffData;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            ApplyDebuffToEnemy(enemy, debuffData);
            enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
            Destroy(gameObject);
        }
    }
    void ApplyDebuffToEnemy(Enemy enemy, DebuffData debuffData)
    {
        switch (debuffData.debuffType)
        {
            case DebuffType.Poison:
                enemy.AddDebuff(new PoisonDebuff(
                    debuffData.duration,
                    debuffData.damagePerTick,
                    debuffData.tickInterval));
                break;
            case DebuffType.Burn:
                enemy.AddDebuff(new BurnDebuff(
                    debuffData.duration,
                    debuffData.damagePerTick,
                    debuffData.tickInterval));
                break;
            case DebuffType.Stun:
                enemy.AddDebuff(new StunDebuff(
                    debuffData.duration
                    ));
                break;
            case DebuffType.Slow:
                enemy.AddDebuff(new SlowDebuff(
                    debuffData.duration,
                    debuffData.slowAmount));
                break;
        }
    }
}