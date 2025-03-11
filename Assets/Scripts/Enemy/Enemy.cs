using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthModifier;
    public int currentHealth;
    private int minHealth = 100;
    public float knockbackForce = 5f;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

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
        
    }
    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplyKnockback(attackerPosition);
        }
        DamageNumberController.instance.SpawnDamage(damage, transform.position);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        DamageNumberController.instance.SpawnDamage(damage, transform.position);
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
    void Die()
    {
        Destroy(gameObject);
    }
}
