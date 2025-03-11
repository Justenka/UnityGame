using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100, maxMana = 100, maxStamina = 100;
    public float currentHealth, currentMana, currentStamina = 100;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public StaminaBar staminaBar;

    private Coroutine rechargeStam, rechargeMana;

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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthBar.SetHealth(currentHealth);
    }
    public void UseMana(float amount)
    {
        currentMana -= amount;
        if (currentMana < 0) currentMana = 0;
        manaBar.SetMana(currentMana);
        if (rechargeMana != null) StopCoroutine(rechargeMana);
        rechargeMana = StartCoroutine(RegenerateMana());
    }
    private IEnumerator RegenerateMana()
    {
        yield return new WaitForSeconds(1f);
        float manaRegenRate = maxMana / 10;
        while (currentMana < maxMana)
        {
            currentMana += manaRegenRate / 10f;
            manaBar.SetMana(currentMana);
            if (currentMana > maxMana) currentMana = maxMana;
            yield return new WaitForSeconds(.1f);

        }
    }
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0;
        staminaBar.SetStamina(currentStamina);
        if (rechargeStam != null) StopCoroutine(rechargeStam);
        rechargeStam = StartCoroutine(RechargeStamina());
    }
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        float ChargeRate = maxStamina / 10;
        while (currentStamina < maxStamina)
        {
            currentStamina += ChargeRate / 10f;
            staminaBar.SetStamina(currentStamina);
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            yield return new WaitForSeconds(.1f);

        }
    }
}
