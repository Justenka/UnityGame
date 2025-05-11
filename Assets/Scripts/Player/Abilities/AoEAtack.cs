using System.Collections;
using UnityEngine;

public class AOEAbility : MonoBehaviour
{
    public float radius = 5f;
    public float damage = 100f;
    public KeyCode abilityKey = KeyCode.Q;
    public float cooldown = 3f;
    public float StaminaCost = 10f;
    public float ManaCost = 10f;

    private float nextUseTime = 0f;
    PlayerAudioManager audioManager;
    Player player;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
        player = GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && Time.time >= nextUseTime)
        {
            UseAOE();
            nextUseTime = Time.time + cooldown;
        }
    }

    void UseAOE()
    {

        // Find enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius);

        // Damage enemies in range
        foreach (Collider2D col in hitEnemies)
        {
            Enemy enemy = col.GetComponent<Enemy>();

            if (enemy != null && !enemy.isDead && player.stats[StatType.Mana].currentValue > ManaCost && player.stats[StatType.Stamina].currentValue > StaminaCost)
            {
                player.UseMana(StaminaCost);
                player.UseStamina(ManaCost);
                enemy.TakeDamage(damage);
            }
        }
        audioManager.PlaySound(audioManager.AoE);
    }

    // Used to visualize range in scene editor. Click on player to see.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public void TriggerAoE(GameObject user)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(user.transform.position, radius);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && player.stats[StatType.Mana].currentValue > ManaCost && player.stats[StatType.Stamina].currentValue > StaminaCost)
            {
                player.UseMana(StaminaCost);
                player.UseStamina(ManaCost);
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        audioManager.PlaySound(audioManager.AoE);
    }
}
