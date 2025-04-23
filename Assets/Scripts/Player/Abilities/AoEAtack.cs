using System.Collections;
using UnityEngine;

public class AOEAbility : MonoBehaviour
{
    public float radius = 5f;
    public float damage = 100f;
    public KeyCode abilityKey = KeyCode.Q;
    public float cooldown = 3f;

    private float nextUseTime = 0f;

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

            if (enemy != null && !enemy.isDead)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    // Used to visualize range in scene editor. Click on player to see.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
