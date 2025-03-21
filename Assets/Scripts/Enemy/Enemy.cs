using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthModifier;
    public int currentHealth;
    private int minHealth = 100;
    public int damageToPlayer = 10;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    private Player playerInTrigger;

    private bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    public Animator animator;

    public GameObject experiencePickupPrefab;
    public int baseXPDrop = 10;
    public float xpMultiplier = 0.1f;

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
    public void TakeDamage(int damage, Vector2 attackerPosition, float knockbackForce, bool KnockBack)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else if(KnockBack)
        {
            animator.SetBool("isHit", true);
            ApplyKnockback(attackerPosition, knockbackForce);
            StartInvincibility();
        }
        DamageNumberController.instance.SpawnDamage(damage, transform.position, false);
    }
    void ApplyKnockback(Vector2 attackerPosition, float knockbackForce)
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
        animator.SetBool("isHit", false);
    }
    void Die()
    {
        DropXP();

        // Find the Player object and give currency
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player != null)
            {
                // Increase the player's currency by, say, 5
                player.GetCurrency(5);
            }
        }

        // Destroy this enemy
        Destroy(gameObject);
    }
    void DropXP()
    {
        if (experiencePickupPrefab != null)
        {
            int xpToDrop = Mathf.RoundToInt(baseXPDrop + (currentHealth * xpMultiplier));
            GameObject xpPickup = Instantiate(experiencePickupPrefab, transform.position, Quaternion.identity);
            xpPickup.GetComponent<ExperiencePickup>().expValue = xpToDrop;
        }
    }
}
