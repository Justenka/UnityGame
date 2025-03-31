using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthModifier;
    public int currentHealth;
    private int minHealth = 100;
    public int damageToPlayer = 10;
    public Rigidbody2D rb;

    public float knockbackForce = 10f;
    public bool isKnockedBack = false;

    public float attackCooldown = 1.5f;
    public float lastAttackTime;
    public Player playerInTrigger;

    public bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    public Animator animator;

    public GameObject experiencePickupPrefab;
    public int baseXPDrop = 10;
    public float xpMultiplier = 0.1f;

    public bool isDead = false;

    public void Start()
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

            GetComponent<SimpleEnemyAI>().enabled = false;

            Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = knockbackDirection * knockbackForce;

            StartCoroutine(HitFreezeDelayed(0.3f, 0.20f));
            Invoke("ResetKnockback", 0.3f);
        }
    }

    IEnumerator HitFreezeDelayed(float delayBeforeFreeze, float freezeDuration)
    {
        yield return new WaitForSeconds(delayBeforeFreeze);
        rb.simulated = false;

        yield return new WaitForSeconds(freezeDuration);
        rb.simulated = true;

        GetComponent<SimpleEnemyAI>().enabled = true;
    }

    void ResetKnockback()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            isKnockedBack = false;

            
        }
    }
    public void StartInvincibility()
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
        if (isDead) return;
        isDead = true;

        DropXP();

        animator.SetTrigger("Die");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player != null)
            {
                player.GetCurrency(5);
            }
        }
        GetComponent<SimpleEnemyAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestroyAfterDeath());
    }

    IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(0.4f);
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
