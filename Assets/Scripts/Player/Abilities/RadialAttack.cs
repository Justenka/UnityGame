using System.Collections;
using UnityEngine;

public class RadialAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectileCount = 36;
    public float duration = 1.5f;
    public float fireInterval = 0.1f;
    public float invincibleTime = 1.6f;
    public float projectileSpeed = 10f;
    public KeyCode abilityKey = KeyCode.R;

    private bool isShooting = false;
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && !isShooting)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    IEnumerator ActivateAbility()
    {
        isShooting = true;

        // Invincible!
        player.isInvincible = true;
        yield return StartCoroutine(SpinAndFire());

        player.isInvincible = false;
        isShooting = false;
    }

    IEnumerator SpinAndFire()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            FireProjectilesInCircle();
            yield return new WaitForSeconds(fireInterval);
            elapsed += fireInterval;
        }
    }

    void FireProjectilesInCircle()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * (360f / projectileCount);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
        }
    }
}
