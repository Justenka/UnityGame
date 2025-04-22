using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float maxHealthModifier;
    public float currentHealth;
    private float minHealth = 100;
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
    public float baseXPDrop = 10;
    public float xpMultiplier = 0.1f;

    public bool isDead = false;
    private EnemyDropItem dropItem;

    public HealthBar healthBar;
    public DebuffData debuffToApply;
    private Dictionary<System.Type, Debuff> activeDebuffs = new();
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dropItem = GetComponent<EnemyDropItem>();

        if (maxHealthModifier != 0)
        {
            currentHealth = Random.Range(50, 300) * maxHealthModifier;
        }
        else
        {
            currentHealth = minHealth;
        }
        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(currentHealth);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (playerInTrigger != null && Time.time >= lastAttackTime + attackCooldown && !playerInTrigger.isInvincible)
        {
            playerInTrigger.TakeDamage(damageToPlayer);
            lastAttackTime = Time.time;
            ApplyDebuffToPlayer(playerInTrigger);
        }
        UpdateDebuffs();
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
                ApplyDebuffToPlayer(player);
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
    public void TakeDamage(float damage, Vector2 attackerPosition, float knockbackForce, bool KnockBack)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        else if (KnockBack)
        {
            animator.SetBool("isHit", true);
            ApplyKnockback(attackerPosition, knockbackForce);
            StartInvincibility();
        }

        if (DamageNumberController.instance != null)
        {
            DamageNumberController.instance.SpawnDamage(damage, transform.position, false);
        }
    }
    public override void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (DamageNumberController.instance != null)
        {
            DamageNumberController.instance.SpawnDamage(damage, transform.position, false);
        }
        // Knockback logic remains in the TakeDamage function in Enemy
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

        GetComponent<SimpleEnemyAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestroyAfterDeath());
    }

    IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(0.4f);
        if (dropItem != null)
        {
            dropItem.DropAll();
        }
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
    void ApplyDebuffToPlayer(Player player)
    {
        if (player == null) return;
        switch (debuffToApply.debuffType)
        {
            case DebuffType.Poison:
                player.AddDebuff(new PoisonDebuff(
                    debuffToApply.duration,
                    debuffToApply.damagePerTick,
                    debuffToApply.tickInterval));
                break;
            case DebuffType.Burn:
                player.AddDebuff(new BurnDebuff(
                    debuffToApply.duration,
                    debuffToApply.damagePerTick,
                    debuffToApply.tickInterval));
                break;
            case DebuffType.Slow:
                player.AddDebuff(new SlowDebuff(
                    debuffToApply.duration,
                    debuffToApply.slowAmount));
                break;
        }
    }

    public override void AddDebuff(Debuff newDebuff)
    {
        System.Type debuffType = newDebuff.GetType();

        if (activeDebuffs.TryGetValue(debuffType, out var existingDebuff))
        {
            existingDebuff.Remove(this);
            activeDebuffs.Remove(debuffType);
        }

        newDebuff.Apply(this);
        activeDebuffs[debuffType] = newDebuff;
    }

    public override void RemoveDebuff(System.Type debuffType)
    {
        if (activeDebuffs.TryGetValue(debuffType, out var debuffToRemove))
        {
            debuffToRemove.Remove(this);
            activeDebuffs.Remove(debuffType);
        }
    }

    public override void UpdateDebuffs()
    {
        List<System.Type> expired = new();
        foreach (var kvp in activeDebuffs)
        {
            kvp.Value.Update(this);
            if (kvp.Value.IsExpired)
            {
                expired.Add(kvp.Key);
            }
        }

        foreach (var type in expired)
        {
            activeDebuffs.Remove(type);
        }
    }

    public override Dictionary<System.Type, Debuff> GetActiveDebuffs()
    {
        return activeDebuffs;
    }
}
