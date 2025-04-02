using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Player player;

    public void DoShoot(WeaponItem weapon)
    {
        if (weapon.projectilePrefab == null || player.stats[StatType.Mana].currentValue < weapon.manaCost)
            return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;
        Vector3 adjustedFirePoint = transform.position + new Vector3(0.5f, 1f, 0);

        GameObject projectile = Instantiate(weapon.projectilePrefab, adjustedFirePoint, Quaternion.identity);
        Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();

        if (rigidbody != null)
        {
            rigidbody.linearVelocity = shootDirection * weapon.attackSpeed;
        }

        player.UseMana(weapon.manaCost);
    }
}