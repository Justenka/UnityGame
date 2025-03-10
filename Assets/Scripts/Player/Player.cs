using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100, maxMana = 100;
    public int currentHealth, currentMana;
    public HealthBar healthBar;
    public ManaBar manaBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        healthBar.SetHealth(currentHealth);
    }
    public void UseMana(int amount)
    {
        currentMana -= amount;

        manaBar.SetMana(currentMana);
    }
}
