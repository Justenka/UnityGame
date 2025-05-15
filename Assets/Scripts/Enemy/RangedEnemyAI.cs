using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    public Player player;
    public float speed = 2f;
    public float retreatDistance = 3f;     // Too close
    public float stopDistance = 6f;        // Ideal shooting range
    public float chaseDistance = 12f;      // Too far
    public float despawnDistance = 80f;

    private Rigidbody2D rb;

    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.GetComponent<Player>();
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // Despawn if too far
        if (distance >= despawnDistance)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (distance < retreatDistance)
        {
            // Too close – retreat (set negative velocity)
            rb.linearVelocity = -direction * speed;
        }
        else if (distance > stopDistance && distance < chaseDistance)
        {
            // Too far – approach (set positive velocity)
            rb.linearVelocity = direction * speed;
        }
        else
        {
            // Stop moving when in ideal range
            rb.linearVelocity = Vector2.zero;
        }
    }
}
