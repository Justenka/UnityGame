using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject Projectile;
    private Transform firePoint;
    public float projectileSpeed = 10f;

    public float manaCost = 10;
    public float maxMana = 100;
    public float currentMana;
    public ManaBar manaBar;

    public float manaRegenRate = 30;
    private Coroutine recharge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = GameObject.FindGameObjectWithTag("Player").transform;
        manaBar = FindAnyObjectByType<ManaBar>();
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
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
        if(Projectile != null && firePoint != null && manaBar != null && currentMana >= manaCost) 
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
            UseMana(manaCost);
        }
    }
    public void UseMana(float amount)
    {
        currentMana -= amount;
        if (currentMana < 0) currentMana = 0;
        manaBar.SetMana(currentMana);
        if (recharge != null) StopCoroutine(recharge);
        recharge = StartCoroutine(RegenerateMana());
    }
    private IEnumerator RegenerateMana()
    {
        yield return new WaitForSeconds(1f);

        while (currentMana < maxMana)
        {
            currentMana += manaRegenRate / 10f;
            manaBar.SetMana(currentMana);
            if (currentMana > maxMana) currentMana = maxMana;
            yield return new WaitForSeconds(.1f);

        }
    }
}
