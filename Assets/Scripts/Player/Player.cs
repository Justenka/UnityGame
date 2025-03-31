using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float maxHealth = 100, maxMana = 100, maxStamina = 100;
    public float currentHealth, currentMana, currentStamina = 100;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public StaminaBar staminaBar;

    private Coroutine rechargeStam, rechargeMana, rechargeHealth;

    public bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    public float currencyHeld;

    public void Start()
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
        if (isInvincible) return;

        UseHealth(amount);

        healthBar.SetHealth(currentHealth);
        
        if (currentHealth < 1)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
        //DamageNumberController.instance.SpawnDamage(amount, transform.position, true);
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
    public void UseHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        healthBar.SetHealth(currentHealth);
        if (rechargeHealth != null) StopCoroutine(rechargeHealth);
        rechargeHealth = StartCoroutine(RechargeHealth());
    }
    private IEnumerator RechargeHealth()
    {
        yield return new WaitForSeconds(1f);
        float ChargeRate = maxHealth / 10;
        while (currentHealth < maxHealth)
        {
            currentHealth += ChargeRate / 10f;
            healthBar.SetHealth(currentHealth);
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            yield return new WaitForSeconds(.1f);

        }
    }
    void StartInvincibility()
    {
        isInvincible = true;
        Invoke("EndInvincibility", invincibilityDuration);
    }
    void EndInvincibility()
    {
        isInvincible = false;
    }
    void Die()
    {
        Destroy(gameObject);
    }
    public void GetCurrency (float amount)
    {
        currencyHeld += amount;
    }
    public void RemoveCurrecny (float amount)
    {
        if (currencyHeld != 0)
        {
            currencyHeld -= amount;
            if (currencyHeld < 0) currencyHeld = 0;
        }
    }
}
