using UnityEngine;

public class RangedEnemy : Enemy
{
    public float attackRange = 10f;
    public GameObject projectilePrefab;
    public GameObject projectilePrefabSpecial;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    private RangedEnemyAI enemyAi;
    public float specialAttackInterval = 5f;
    public int radialProjectileCount = 8;
    private float nextSpecialAttackTime;

    public new void Start()
    {
        if (animator != null)
        {
            //bool hasXVelocity = animator.HasParameter("xVelocity");
            //Debug.Log("Does animator have xVelocity? " + hasXVelocity);

            //if (!hasXVelocity)
            //    Debug.LogError("Animator is missing 'xVelocity' parameter!");
        }

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
        if (isDead)
            animator.SetTrigger("Die");

        if (Time.time >= nextSpecialAttackTime)
        {
            SpecialAttack();
            nextSpecialAttackTime = Time.time + specialAttackInterval;
            return; // Skip normal attack this frame
        }

        float distanceToPlayer = Vector2.Distance(transform.position, enemyAi.player.transform.position);
        // Check if the player is within the attack range
        if (distanceToPlayer <= attackRange)
        {
            OnPlayerEnterAttackRange(enemyAi.player);
            Attack(); // Call the Attack method if in range
        }
    }
    void FixedUpdate()
    {
        if (animator != null && rb != null)
        {
            // Get the velocity
            Vector2 velocity = rb.linearVelocity;

            // Calculate speed (absolute value for animator)
            float xSpeed = Mathf.Abs(velocity.magnitude);
            animator.SetFloat("xVelocity", xSpeed);

            // Flip logic (only when moving significantly)
            if (Mathf.Abs(velocity.x) > 0.1f) // Threshold to prevent flipping at tiny velocities
            {
                // Get the current scale
                Vector3 scale = transform.localScale;

                // Flip based on X velocity direction
                scale.x = Mathf.Sign(velocity.x) * Mathf.Abs(scale.x);

                // Apply the new scale
                transform.localScale = scale;
            }

            Debug.Log($"Velocity: {velocity} | Speed: {xSpeed} | Facing: {(velocity.x > 0 ? "Right" : "Left")}");
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

    private void SpecialAttack()
    {
        if (gameObject.tag != "Boss")
            return;

        if (enemyAi != null)
            enemyAi.enabled = false;

        float angleStep = 360f / radialProjectileCount;
        float angle = 0f;

        for (int i = 0; i < radialProjectileCount; i++)
        {
            float projectileDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 shootDirection = new Vector2(projectileDirX, projectileDirY).normalized;

            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject projectile = Instantiate(projectilePrefabSpecial, firePoint.position, rotation);

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
            }

            angle += angleStep;
        }

        // Re-enable AI after a delay (e.g., 1 second)
        Invoke(nameof(EnableAI), 1f);
        // Optionally play a special attack animation or effect
        // animator.SetTrigger("SpecialAttack");
    }

    private void EnableAI()
    {
        if (enemyAi != null)
            enemyAi.enabled = true;
    }

}
