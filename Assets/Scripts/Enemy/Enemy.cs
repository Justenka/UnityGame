using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthModifier;
    public int currentHealth;
    //private int maxHealth = 100;

    void Start()
    {
        currentHealth = Random.Range(50,300)*maxHealthModifier;
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
