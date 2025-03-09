using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthModifier;
    public int currentHealth;
    private int minHealth = 100;

    void Start()
    {
        if (maxHealthModifier != 0)
        {
            currentHealth = Random.Range(50, 300) * maxHealthModifier;
        }
        else
        {
            currentHealth = minHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
