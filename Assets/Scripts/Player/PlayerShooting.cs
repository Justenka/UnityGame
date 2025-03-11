using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject Projectile;
    private Transform firePoint;
    public float projectileSpeed = 10f;
    public Player player;
    public float manaCost = 10;
    private Coroutine recharge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        if(Projectile != null && firePoint != null && player.manaBar != null && player.currentMana >= manaCost) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector2 shootDirection = (mousePosition - firePoint.position).normalized;

            GameObject projectile = Instantiate(Projectile, firePoint.position, Quaternion.identity);
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();

            if(rigidbody != null)
            {

                rigidbody.linearVelocity = shootDirection * projectileSpeed;
            }
            player.UseMana(manaCost);
        }
    }
}
