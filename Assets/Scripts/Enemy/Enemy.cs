using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthModifier;
    public int currentHealth;
    private int minHealth = 100;
    public int damageToPlayer = 10;
    public float knockbackForce = 5f;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    private Player playerInTrigger;

    private bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (maxHealthModifier != 0)
        {
            currentHealth = Random.Range(50, 300) * maxHealthModifier;
        }
        else
        {
            currentHealth = minHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger != null && Time.time >= lastAttackTime + attackCooldown)
        {
            playerInTrigger.TakeDamage(damageToPlayer);
            lastAttackTime = Time.time;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                playerInTrigger = player;
                playerInTrigger.TakeDamage(damageToPlayer);
                lastAttackTime = Time.time;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = null;
        }
    }
    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplyKnockback(attackerPosition);
            StartInvincibility();
        }
        DamageNumberController.instance.SpawnDamage(damage, transform.position, false);
    }
    void ApplyKnockback(Vector2 attackerPosition)
    {
        if (rb != null && !isKnockedBack)
        {
            isKnockedBack = true;
            Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            Invoke("ResetKnockback", 0.5f);
        }
    }

    void ResetKnockback()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            isKnockedBack = false;
        }
    }
    void StartInvincibility()
    {
        isInvincible = true;
        Invoke("EndInvincibility", invincibilityDuration);
    }
    void EndInvincibility()
    {
        isInvincible = false;
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
