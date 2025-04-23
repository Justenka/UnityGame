using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 10; // Default damage value, can be set by the attacker
    public float speed = 10f; // Initial speed of the projectile
    public Vector2 direction; // Direction the projectile will move in
    public DebuffData debuffToApply;
    // Optional: public DebuffData debuffToApply;

    private Rigidbody2D rb;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // If direction is not set externally, try to infer from initial velocity
        if (direction == Vector2.zero && rb.linearVelocity != Vector2.zero)
        {
            direction = rb.linearVelocity.normalized;
        }
        // If there's a direction, set the velocity
        else if (direction != Vector2.zero)
        {
            rb.linearVelocity = direction * speed;
        }

        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifetime);
    }
    //void Update()
    //{
    //    // Calculate the angle of the velocity vector
    //    float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;

    //    // Rotate the projectile to face that angle
    //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return; // Prevent multiple hits

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            hasHit = true;
            player.TakeDamage(damage);
            // Optional: Apply debuff to player if debuffToApply is set
            if (debuffToApply != null)
            {
                ApplyDebuffToPlayer(player);
            }
            Destroy(gameObject); // Destroy the projectile on hit
        }
        // Destroy the projectile if it hits a non-trigger object that isn't the enemy
        else if (!collision.isTrigger && !collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
    void ApplyDebuffToPlayer(Player player)
    {
        if (player == null || debuffToApply == null) return;
        switch (debuffToApply.debuffType)
        {
            case DebuffType.Poison:
                player.AddDebuff(new PoisonDebuff(debuffToApply.duration, debuffToApply.damagePerTick, debuffToApply.tickInterval));
                break;
            case DebuffType.Burn:
                player.AddDebuff(new BurnDebuff(debuffToApply.duration, debuffToApply.damagePerTick, debuffToApply.tickInterval));
                break;
            case DebuffType.Slow:
                player.AddDebuff(new SlowDebuff(debuffToApply.duration, debuffToApply.slowAmount));
                break;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return; // Prevent multiple hits

        // Handle collisions with non-trigger objects (like walls)
        if (!collision.collider.CompareTag("Enemy"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }

    // You might want to add visual effects on hit (e.g., an explosion)
    // void OnDestroy()
    // {
    //     // Instantiate a visual effect prefab here
    // }
}