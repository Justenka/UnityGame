using UnityEngine;

public class CompanionAI : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float maxDistanceFromPlayer = 10f;
    public float enemySearchRadius = 8f;
    public float attackRange = 1f;
    public float damage = 50f;
    public float attackCooldown = 1.5f;

    public Transform player;
    private Transform currentTarget;
    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Stay near player
        if (Vector2.Distance(transform.position, player.position) > maxDistanceFromPlayer)
        {
            MoveToward(player.position);
            currentTarget = null;
            return;
        }

        // Find or check target
        if (currentTarget == null || !IsEnemyValid(currentTarget))
            currentTarget = FindClosestEnemy();

        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.position);
            if (distance > attackRange)
            {
                MoveToward(currentTarget.position);
            }
            else if (cooldownTimer <= 0f)
            {
                Attack(currentTarget);
            }
        }
        else
        {
            FollowPlayer();
        }
    }

    void MoveToward(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }

    void FollowPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) > 1f)
            MoveToward(player.position);
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            float distToPlayer = Vector2.Distance(player.position, enemy.transform.position);

            if (dist < enemySearchRadius && distToPlayer < maxDistanceFromPlayer && dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    bool IsEnemyValid(Transform enemy)
    {
        return enemy != null && enemy.gameObject.activeInHierarchy;
    }

    void Attack(Transform enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
            cooldownTimer = attackCooldown;
            currentTarget = null;
        }
    }
}
