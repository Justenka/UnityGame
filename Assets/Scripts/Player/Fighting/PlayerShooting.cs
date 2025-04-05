using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Player player;
    private float shootCooldown = 0f;

    void Update()
    {
        if (shootCooldown > 0f)
            shootCooldown -= Time.deltaTime;
        if (Input.GetMouseButton(0)) // Left click held
        {
            WeaponItem weapon = player.GetComponent<EquipmentManager>()?.equippedWeapon;
            if (weapon != null && weapon.actionType == ActionType.Ranged)
            {
                DoShoot(weapon);
            }
        }
    }

    public void DoShoot(WeaponItem weapon)
    {
        // Block if still on cooldown
        if (shootCooldown > 0f)
            return;

        if (weapon.projectilePrefab == null || player.stats[StatType.Mana].currentValue < weapon.manaCost)
            return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - transform.position).normalized;
        Vector3 adjustedFirePoint = transform.position + new Vector3(0.5f, 1f, 0);

        GameObject projectile = Instantiate(weapon.projectilePrefab, adjustedFirePoint, Quaternion.identity);
        Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (rigidbody != null)
        {
            rigidbody.linearVelocity = shootDirection * weapon.projectileSpeed;
        }

        if (projScript != null)
        {
            projScript.damage = player.stats[StatType.Attack].Total;
        }

        player.UseMana(weapon.manaCost);

        // Set cooldown based on fire rate (shots per second)
        shootCooldown = 1f / weapon.fireRate; //So if fireRate = 2, you'll be able to shoot once every 0.5 seconds.
    }

}