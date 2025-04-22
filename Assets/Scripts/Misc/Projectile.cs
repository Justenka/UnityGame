using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    public float lifetime = 10f;
    public float knockBack = 1.5f;
    public bool doesKnockBack = true;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
            Destroy(gameObject);
        }
    }
}