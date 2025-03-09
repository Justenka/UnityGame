using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int maxStamina = 100;
    public int currentStamina;
    public int maxMana = 100;
    public int currentMana;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public StaminaBar staminaBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
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
    public void UseStamina(int amount)
    {
        currentStamina -= amount;

        staminaBar.SetStamina(currentStamina);
    }
}
