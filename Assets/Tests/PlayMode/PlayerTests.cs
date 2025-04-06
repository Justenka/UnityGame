using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using TMPro;

public class SpyHealthBar : HealthBar
{
    public bool SetHealthCalled = false;
    public float LastHealthValue;

    public override void SetHealth(float value)
    {
        SetHealthCalled = true;
        LastHealthValue = value;
        base.SetHealth(value);
    }
}

public class SpyManaBar : ManaBar
{
    public bool SetManaCalled = false;
    public float LastManaValue;

    public override void SetMana(float value)
    {
        SetManaCalled = true;
        LastManaValue = value;
        base.SetMana(value);
    }
}

public class SpyStaminaBar : StaminaBar
{
    public bool SetStaminaCalled = false;
    public float LastStaminaValue;

    public override void SetStamina(float value)
    {
        SetStaminaCalled = true;
        LastStaminaValue = value;
        base.SetStamina(value);
    }
}

public class PlayerTests
{
    private GameObject playerGameObject;
    private Player player;
    private HealthBar mockHealthBar;
    private ManaBar mockManaBar;
    private StaminaBar mockStaminaBar;

    [SetUp]
    public void Setup()
    {
        playerGameObject = new GameObject("Player");
        player = playerGameObject.AddComponent<Player>();

        GameObject healthBarObject = new GameObject("HealthBar");
        mockHealthBar = healthBarObject.AddComponent<SpyHealthBar>();
        mockHealthBar.healthSlider = healthBarObject.AddComponent<Slider>();
        player.healthBar = mockHealthBar;

        GameObject manaBarObject = new GameObject("ManaBar");
        mockManaBar = manaBarObject.AddComponent<SpyManaBar>();
        mockManaBar.manaSlider = manaBarObject.AddComponent<Slider>();
        player.manaBar = mockManaBar;

        GameObject staminaBarObject = new GameObject("StaminaBar");
        mockStaminaBar = staminaBarObject.AddComponent<SpyStaminaBar>();
        mockStaminaBar.staminaSlider = staminaBarObject.AddComponent<Slider>();
        player.staminaBar = mockStaminaBar;

        GameObject userNameObject = new GameObject("UserNameText");
        TextMeshProUGUI userNameText = userNameObject.AddComponent<TextMeshProUGUI>();
        player.userName = userNameText;

        player.stats[StatType.Health].baseValue = 100;
        player.stats[StatType.Defense].baseValue = 0;
        

        //player.Awake();
        player.Start();
    }

    [TearDown]
    public void Teardown()
    {
        PlayerPrefs.DeleteKey("PlayerUsername");
        Object.DestroyImmediate(playerGameObject);
        Object.DestroyImmediate(player.healthBar.gameObject);
        Object.DestroyImmediate(player.manaBar.gameObject);
        Object.DestroyImmediate(player.staminaBar.gameObject);
    }

    [Test]
    public void Start_InitializesStatsCorrectly()
    {
        Assert.AreEqual(player.stats[StatType.Health].Total, player.stats[StatType.Health].currentValue);
        Assert.AreEqual(player.stats[StatType.Mana].Total, player.stats[StatType.Mana].currentValue);
        Assert.AreEqual(player.stats[StatType.Stamina].Total, player.stats[StatType.Stamina].currentValue);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        
        player.TakeDamage(20);
        
        Assert.AreEqual(80f, player.stats[StatType.Health].currentValue, "Players Health" + player.stats[StatType.Health].currentValue + " Current Defense " + player.stats[StatType.Defense].currentValue);
    }

    [UnityTest]
    public IEnumerator TakeDamage_KillsPlayerWhenHealthIsZero()
    {
        player.TakeDamage(150);
        yield return new WaitForSeconds(1.5f);
        Assert.IsTrue(playerGameObject == null, "Players Health" + player.stats[StatType.Health].currentValue);
    }

    [UnityTest]
    public IEnumerator TakeDamage_Invincibility()
    {
        player.TakeDamage(20);
        Assert.IsTrue(player.isInvincible);
        yield return new WaitForSeconds(player.invincibilityDuration + 0.05f);
        Assert.IsFalse(player.isInvincible);
    }

    [Test]
    public void UseMana_ReducesMana()
    {
        player.UseMana(30);
        Assert.AreEqual(70, player.stats[StatType.Mana].currentValue);
    }

    [Test]
    public void UseMana_ManaCannotGoBelowZero()
    {
        player.UseMana(150);
        Assert.AreEqual(0, player.stats[StatType.Mana].currentValue);
    }

    [UnityTest]
    public IEnumerator UseMana_RegeneratesMana()
    {
        player.UseMana(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.stats[StatType.Mana].currentValue, 50);
    }

    [Test]
    public void UseStamina_ReducesStamina()
    {
        player.UseStamina(40);
        Assert.AreEqual(60, player.stats[StatType.Stamina].currentValue);
    }

    [Test]
    public void UseStamina_StaminaCannotGoBelowZero()
    {
        player.UseStamina(120);
        Assert.AreEqual(0, player.stats[StatType.Stamina].currentValue);
    }

    [UnityTest]
    public IEnumerator UseStamina_RegeneratesStamina()
    {
        player.UseStamina(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.stats[StatType.Stamina].currentValue, 50);
    }

    [Test]
    public void UseHealth_ReducesHealth()
    {
        player.UseHealth(30);
        Assert.AreEqual(70, player.stats[StatType.Health].currentValue);
    }

    [Test]
    public void UseHealth_HealthCannotGoBelowZero()
    {
        player.UseHealth(150);
        Assert.AreEqual(0, player.stats[StatType.Health].currentValue);
    }

    [UnityTest]
    public IEnumerator UseHealth_RegeneratesHealth()
    {
        player.UseHealth(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.stats[StatType.Health].currentValue, 50);
    }

    [Test]
    public void GetCurrency_IncreasesCurrency()
    {
        player.GetCurrency(50);
        Assert.AreEqual(50, player.currencyHeld);
    }

    [Test]
    public void RemoveCurrency_DecreasesCurrency()
    {
        player.currencyHeld = 100;
        player.RemoveCurrency(30);
        Assert.AreEqual(70, player.currencyHeld);
    }

    [Test]
    public void RemoveCurrency_DoesNotGoBelowZero()
    {
        player.currencyHeld = 10;
        player.RemoveCurrency(20);
        Assert.AreEqual(0, player.currencyHeld);
    }

    [Test]
    public void UseMana_CallsSetManaOnManaBar()
    {
        player.UseMana(25);
        var spy = mockManaBar as SpyManaBar;
        Assert.IsTrue(spy.SetManaCalled);
        Assert.AreEqual(75, spy.LastManaValue);
    }

    [Test]
    public void UseStamina_CallsSetStaminaOnStaminaBar()
    {
        player.UseStamina(40);
        var spy = mockStaminaBar as SpyStaminaBar;
        Assert.IsTrue(spy.SetStaminaCalled);
        Assert.AreEqual(60, spy.LastStaminaValue);
    }

    [Test]
    public void UseHealth_CallsSetHealthOnHealthBar()
    {
        player.UseHealth(10);
        var spy = mockHealthBar as SpyHealthBar;
        Assert.IsTrue(spy.SetHealthCalled);
        Assert.AreEqual(90, spy.LastHealthValue);
    }

    [Test]
    public void UseMana_BeyondAvailable_CallsSetManaWithZero()
    {
        player.stats[StatType.Mana].currentValue = 10;
        player.UseMana(50);
        var spy = mockManaBar as SpyManaBar;
        Assert.IsTrue(spy.SetManaCalled);
        Assert.AreEqual(0, spy.LastManaValue);
    }

    [Test]
    public void UseStamina_BeyondAvailable_CallsSetStaminaWithZero()
    {
        player.stats[StatType.Stamina].currentValue = 10;
        player.UseStamina(50);
        var spy = mockStaminaBar as SpyStaminaBar;
        Assert.IsTrue(spy.SetStaminaCalled);
        Assert.AreEqual(0, spy.LastStaminaValue);
    }

    [Test]
    public void UseHealth_BeyondAvailable_CallsSetHealthWithZero()
    {
        player.stats[StatType.Health].currentValue = 5;
        player.UseHealth(100);
        var spy = mockHealthBar as SpyHealthBar;
        Assert.IsTrue(spy.SetHealthCalled);
        Assert.AreEqual(0, spy.LastHealthValue);
    }

    [Test]
    public void UseHealth_NegativeAmount_CallsSetHealthWithIncreasedValue()
    {
        player.stats[StatType.Health].currentValue = 50;
        player.UseHealth(-30);
        var spy = mockHealthBar as SpyHealthBar;
        Assert.IsTrue(spy.SetHealthCalled);
        Assert.AreEqual(80, spy.LastHealthValue);
    }

    [Test]
    public void UseMana_NegativeAmount_CallsSetManaWithIncreasedValue()
    {
        player.stats[StatType.Mana].currentValue = 40;
        player.UseMana(-20);
        var spy = mockManaBar as SpyManaBar;
        Assert.IsTrue(spy.SetManaCalled);
        Assert.AreEqual(60, spy.LastManaValue);
    }

    [Test]
    public void UseStamina_NegativeAmount_CallsSetStaminaWithIncreasedValue()
    {
        player.stats[StatType.Stamina].currentValue = 70;
        player.UseStamina(-20);
        var spy = mockStaminaBar as SpyStaminaBar;
        Assert.IsTrue(spy.SetStaminaCalled);
        Assert.AreEqual(90, spy.LastStaminaValue);
    }
}