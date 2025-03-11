using UnityEditor.AssetImporters;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 50;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.TakeDamage(damage, transform.position);
        }
    }
}
