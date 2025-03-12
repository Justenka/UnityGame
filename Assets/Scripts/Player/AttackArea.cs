using UnityEditor.AssetImporters;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 50;
    private float knockBack = 5f;
    private bool doesKnockBack = true;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.TakeDamage(damage, transform.position, knockBack, doesKnockBack);
        }
    }
}
