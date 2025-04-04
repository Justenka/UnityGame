using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;
    public ManaBar manaBar;
    public StaminaBar staminaBar;

    private Coroutine rechargeStam, rechargeMana, rechargeHealth;

    public bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    public float currencyHeld;

    public Dictionary<StatType, StatValue> stats = new();
    public string Name;
    public TMP_Text userName;
    public void Awake()
    {
        InitializeStats();
    }

    public void Start()
    {
        if (PlayerPrefs.HasKey("PlayerUsername"))
        {
            Name = PlayerPrefs.GetString("PlayerUsername");
            if(Name != "")
            {
                userName.text = Name;
            }
        }
        SetInitialValues();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            GetComponent<EquipmentManager>().UseWeapon(gameObject);
        }

        // Rest of your update logic...
    }
    void LateUpdate()
    {
        userName.transform.rotation = Quaternion.identity;
    }
    void InitializeStats()
    {
        stats[StatType.Health] = new StatValue { baseValue = 100 };
        stats[StatType.Mana] = new StatValue { baseValue = 100 };
        stats[StatType.Stamina] = new StatValue { baseValue = 100 };
        stats[StatType.Attack] = new StatValue { baseValue = 10 };
        stats[StatType.Defense] = new StatValue { baseValue = 5 };
        stats[StatType.Speed] = new StatValue { baseValue = 5 };
    }

    void SetInitialValues()
    {
        float maxHealth = stats[StatType.Health].Total;
        float maxMana = stats[StatType.Mana].Total;
        float maxStamina = stats[StatType.Stamina].Total;

        stats[StatType.Health].currentValue = maxHealth;
        stats[StatType.Mana].currentValue = maxMana;
        stats[StatType.Stamina].currentValue = maxStamina;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);

        manaBar.SetMaxMana(maxMana);
        manaBar.SetMana(maxMana);

        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetStamina(maxStamina);
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        float defense = stats[StatType.Defense].Total;
        float damageTaken = Mathf.Max(0, amount - defense);

        UseHealth(damageTaken);

        if (stats[StatType.Health].currentValue < 1)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
    }

    public void UseMana(float amount)
    {
        StatValue mana = stats[StatType.Mana];
        mana.currentValue -= amount;
        if (mana.currentValue < 0) mana.currentValue = 0;
        manaBar.SetMana(mana.currentValue);

        if (rechargeMana != null) StopCoroutine(rechargeMana);
        rechargeMana = StartCoroutine(RegenerateMana());
    }

    private IEnumerator RegenerateMana()
    {
        yield return new WaitForSeconds(1f);
        float regenRate = stats[StatType.Mana].Total / 10;
        while (stats[StatType.Mana].currentValue < stats[StatType.Mana].Total)
        {
            stats[StatType.Mana].currentValue += regenRate / 10f;
            manaBar.SetMana(stats[StatType.Mana].currentValue);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UseStamina(float amount)
    {
        StatValue stam = stats[StatType.Stamina];
        stam.currentValue -= amount;
        if (stam.currentValue < 0) stam.currentValue = 0;
        staminaBar.SetStamina(stam.currentValue);

        if (rechargeStam != null) StopCoroutine(rechargeStam);
        rechargeStam = StartCoroutine(RechargeStamina());
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        float regenRate = stats[StatType.Stamina].Total / 10;
        while (stats[StatType.Stamina].currentValue < stats[StatType.Stamina].Total)
        {
            stats[StatType.Stamina].currentValue += regenRate / 10f;
            staminaBar.SetStamina(stats[StatType.Stamina].currentValue);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UseHealth(float amount)
    {
        StatValue hp = stats[StatType.Health];
        hp.currentValue -= amount;

        if (hp.currentValue > hp.Total)
            hp.currentValue = hp.Total;

        if (hp.currentValue < 0)
            hp.currentValue = 0;

        healthBar.SetHealth(hp.currentValue);

        if (rechargeHealth != null) StopCoroutine(rechargeHealth);
        rechargeHealth = StartCoroutine(RechargeHealth());
    }

    private IEnumerator RechargeHealth()
    {
        yield return new WaitForSeconds(1f);
        float regenRate = stats[StatType.Health].Total / 10;
        while (stats[StatType.Health].currentValue < stats[StatType.Health].Total)
        {
            stats[StatType.Health].currentValue += regenRate / 10f;
            healthBar.SetHealth(stats[StatType.Health].currentValue);
            yield return new WaitForSeconds(0.1f);
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

    public void GetCurrency(float amount)
    {
        currencyHeld += amount;
    }

    public void RemoveCurrency(float amount)
    {
        currencyHeld -= amount;
        if (currencyHeld < 0) currencyHeld = 0;
    }
}
