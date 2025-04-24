using UnityEngine;

public class RangedEnemy : Enemy
{
    public float attackRange = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    private RangedEnemyAI enemyAi;

    public new void Start()
    {
        base.Start();
        if (firePoint == null)
        {
            Debug.LogError("Fire Point not assigned for ranged enemy: " + gameObject.name);
            enabled = false;
        }
        enemyAi = gameObject.GetComponent<RangedEnemyAI>();
    }
    public new void Update()
    {
        if (enemyAi.player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, enemyAi.player.transform.position);
        // Check if the player is within the attack range
        if (distanceToPlayer <= attackRange)
        {
            OnPlayerEnterAttackRange(enemyAi.player);
            Attack(); // Call the Attack method if in range
        }
    }

    public override float GetAttackRange()
    {
        return attackRange;
    }

    public override void Attack()
    {
        if (playerInTrigger != null && Time.time >= lastAttackTime + attackCooldown && !playerInTrigger.isInvincible && projectilePrefab != null && firePoint != null)
        {
            lastAttackTime = Time.time;

            Vector3 targetPosition = enemyAi.player.transform.position;
            targetPosition.z = 0f; // Ensure z is the same
            Vector2 shootDirection = (targetPosition - firePoint.position).normalized;

            // Calculate angle in degrees (adjust the +90f if needed)
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 90f;

            // Apply rotation using Quaternion.Euler
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

            if (projectileRB != null)
            {
                projectileRB.linearVelocity = shootDirection * projectileSpeed;
            }

            if (projectileScript != null)
            {
                projectileScript.damage = damageToPlayer;
                projectileScript.debuffToApply = debuffToApply;
                // Optionally, you could add debuff application logic to the projectile here
            }

            // Optionally play an attack animation
            //if (animator != null)
            //{
            //    animator.SetTrigger("Attack"); // Or your ranged attack animation trigger
            //}
        }
    }

    public override void OnPlayerEnterAttackRange(Player player)
    {
        playerInTrigger = player;
        // Ranged enemies might start aiming or preparing to attack
    }

    public override void OnPlayerExitAttackRange(Player player)
    {
        playerInTrigger = null;
        // Ranged enemies might stop aiming
    }

    // Optional: Visualize attack range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
