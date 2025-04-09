using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private Vector2 respawnPosition;
    public GameObject respawnMenu;
    public ExperienceLevelController Experience;
    public DebuffUIManager debuffUIManager;
    private Dictionary<System.Type, Debuff> activeDebuffs = new();


    //[SerializeField]
    //private InputActionReference attack;

    //private WeaponRotation weaponRotation;

    public void Awake()
    {
        InitializeStats();
        //weaponRotation = GetComponentInChildren<WeaponRotation>();
    }

    public virtual void Start()
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
        respawnPosition = transform.position;
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<EquipmentManager>().UseWeapon(gameObject);
        }

        UpdateDebuffs();

    }
    void LateUpdate()
    {
        if(userName != null)
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

    public virtual void TakeDamage(float amount)
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
    public void RestoreMana(int amount)
    {
        StatValue mana = stats[StatType.Mana];
        mana.currentValue = Mathf.Min(mana.currentValue + amount, mana.Total);
        manaBar.SetMana(mana.currentValue);
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
    public void RestoreStamina(int amount)
    {
        StatValue stam = stats[StatType.Stamina];
        stam.currentValue = Mathf.Min(stam.currentValue + amount, stam.Total);
        staminaBar.SetStamina(stam.currentValue);
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
        if (hp.currentValue < 0) hp.currentValue = 0;
        healthBar.SetHealth(hp.currentValue);

        if (rechargeHealth != null) StopCoroutine(rechargeHealth);
        rechargeHealth = StartCoroutine(RechargeHealth());
    }
    public void RestoreHealth(int amount)
    {
        StatValue hp = stats[StatType.Health];
        hp.currentValue = Mathf.Min(hp.currentValue + amount, hp.Total);
        healthBar.SetHealth(hp.currentValue);
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
        gameObject.SetActive(false);
        ShowRespawnMenu();
    }
    void ShowRespawnMenu()
    {
        respawnMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnRespawnButtonClick()
    {
        Respawn();
        gameObject.SetActive(true);
        respawnMenu.SetActive(false);
        Time.timeScale = 1; 
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
    void Respawn()
    {
        transform.position = respawnPosition;
        Experience.currentExperience = 0;
        currencyHeld = 0;
        GetComponent<EquipmentManager>().UnequipAll();
        InventoryManager inventoryManager = FindFirstObjectByType<InventoryManager>();
        inventoryManager.ClearInventory();
        activeDebuffs.Clear();
        SetInitialValues();
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
    private void RefreshStats()
    {
        healthBar.SetMaxHealth(stats[StatType.Health].Total);
        manaBar.SetMaxMana(stats[StatType.Mana].Total);
        staminaBar.SetMaxStamina(stats[StatType.Stamina].Total);

        // Optional: Clamp current values if above new max
        stats[StatType.Health].currentValue = Mathf.Min(stats[StatType.Health].currentValue, stats[StatType.Health].Total);
        stats[StatType.Mana].currentValue = Mathf.Min(stats[StatType.Mana].currentValue, stats[StatType.Mana].Total);
        stats[StatType.Stamina].currentValue = Mathf.Min(stats[StatType.Stamina].currentValue, stats[StatType.Stamina].Total);

        healthBar.SetHealth(stats[StatType.Health].currentValue);
        manaBar.SetMana(stats[StatType.Mana].currentValue);
        staminaBar.SetStamina(stats[StatType.Stamina].currentValue);
    }
    public void ApplyItemStats(Item item)
    {
        foreach (var mod in item.statModifiers)
        {
            if (stats.ContainsKey(mod.statType))
                stats[mod.statType].bonusValue += mod.value;
        }
        RefreshStats();
    }

    public void RemoveItemStats(Item item)
    {
        foreach (var mod in item.statModifiers)
        {
            if (stats.ContainsKey(mod.statType))
                stats[mod.statType].bonusValue -= mod.value;
        }
        RefreshStats();
    }
    public void AddDebuff(Debuff newDebuff)
    {
        System.Type debuffType = newDebuff.GetType();

        // Remove existing one of the same type
        if (activeDebuffs.TryGetValue(debuffType, out var existingDebuff))
        {
            existingDebuff.Remove(this);
            activeDebuffs.Remove(debuffType);
        }

        // Apply and store the new debuff
        newDebuff.Apply(this);
        activeDebuffs[debuffType] = newDebuff;
    }
    public void UpdateDebuffs()
    {
        List<System.Type> expired = new();
        foreach (var kvp in activeDebuffs)
        {
            kvp.Value.Update(this);
            if (kvp.Value.IsExpired)
            {
                kvp.Value.Remove(this);
                expired.Add(kvp.Key);
            }
        }

        foreach (var type in expired)
        {
            activeDebuffs.Remove(type);
        }
    }

    //private void OnEnable()
    //{
    //    attack.action.performed += PerformAttack;
    //}

    //private void OnDisable()
    //{
    //    attack.action.performed -= PerformAttack;
    //}

    //private void PerformAttack(InputAction.CallbackContext obj)
    //{
    //    weaponRotation.Attack();
    //}
}
