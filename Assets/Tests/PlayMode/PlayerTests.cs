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

        player.maxHealth = 100;
        player.maxMana = 100;
        player.maxStamina = 100;
        player.invincibilityDuration = 0.1f; // Shorten for tests
        player.Start(); 
    }


    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerGameObject);
        Object.DestroyImmediate(player.healthBar.gameObject);
        Object.DestroyImmediate(player.manaBar.gameObject);
        Object.DestroyImmediate(player.staminaBar.gameObject);
    }
    //===================================================================================================================//
    //===================================================================================================================//
    // UNIT TEST PART BEGIN//
    //===================================================================================================================//
    //===================================================================================================================//
    [Test]
    public void Start_InitializesHealthManaStamina()
    {
        Assert.AreEqual(player.maxHealth, player.currentHealth);
        Assert.AreEqual(player.maxMana, player.currentMana);
        Assert.AreEqual(player.maxStamina, player.currentStamina);
        Assert.AreEqual(player.maxHealth, mockHealthBar.healthSlider.maxValue);
        Assert.AreEqual(player.maxMana, mockManaBar.manaSlider.maxValue);
        Assert.AreEqual(player.maxStamina, mockStaminaBar.staminaSlider.maxValue);
        Assert.AreEqual(player.currentHealth, mockHealthBar.healthSlider.value);
        Assert.AreEqual(player.currentMana, mockManaBar.manaSlider.value);
        Assert.AreEqual(player.currentStamina, mockStaminaBar.staminaSlider.value);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        player.TakeDamage(20);
        Assert.AreEqual(80, player.currentHealth);
        Assert.AreEqual(80, mockHealthBar.healthSlider.value);
    }

    [UnityTest]
    public IEnumerator TakeDamage_KillsPlayerWhenHealthIsZero()
    {
        player.TakeDamage(100);
        yield return new WaitForSeconds(1.5f);
        Assert.IsTrue(playerGameObject == null);
    }

    [UnityTest]
    public IEnumerator TakeDamage_Invincibility()
    {
        player.TakeDamage(20);
        Assert.IsTrue(player.isInvincible);
        yield return new WaitForSeconds(player.invincibilityDuration + 0.05f); // Add a small buffer
        Assert.IsFalse(player.isInvincible);
    }

    [Test]
    public void UseMana_ReducesMana()
    {
        player.UseMana(30);
        Assert.AreEqual(70, player.currentMana);
        Assert.AreEqual(70, mockManaBar.manaSlider.value);
    }

    [Test]
    public void UseMana_ManaCannotGoBelowZero()
    {
        player.UseMana(150);
        Assert.AreEqual(0, player.currentMana);
    }

    [UnityTest]
    public IEnumerator UseMana_RegeneratesMana()
    {
        player.UseMana(50);
        yield return new WaitForSeconds(1.5f); // Wait for regen to start and tick
        Assert.Greater(player.currentMana, 50);
        Assert.Greater(mockManaBar.manaSlider.value, 50);
    }

    [Test]
    public void UseStamina_ReducesStamina()
    {
        player.UseStamina(40);
        Assert.AreEqual(60, player.currentStamina);
        Assert.AreEqual(60, mockStaminaBar.staminaSlider.value);
    }

    [Test]
    public void UseStamina_StaminaCannotGoBelowZero()
    {
        player.UseStamina(120);
        Assert.AreEqual(0, player.currentStamina);
    }

    [UnityTest]
    public IEnumerator UseStamina_RegeneratesStamina()
    {
        player.UseStamina(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.currentStamina, 50);
        Assert.Greater(mockStaminaBar.staminaSlider.value, 50);
    }

    [Test]
    public void UseHealth_ReducesHealth()
    {
        player.UseHealth(30);
        Assert.AreEqual(70, player.currentHealth);
        Assert.AreEqual(70, mockHealthBar.healthSlider.value);
    }

    [Test]
    public void UseHealth_HealthCannotGoBelowZero()
    {
        player.UseHealth(150);
        Assert.AreEqual(0, player.currentHealth);
    }

    [UnityTest]
    public IEnumerator UseHealth_RegeneratesHealth()
    {
        player.UseHealth(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.currentHealth, 50);
        Assert.Greater(mockHealthBar.healthSlider.value, 50);
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
        player.RemoveCurrecny(30);
        Assert.AreEqual(70, player.currencyHeld);
    }

    [Test]
    public void RemoveCurrency_DoesNotGoBelowZero()
    {
        player.currencyHeld = 10;
        player.RemoveCurrecny(20);
        Assert.AreEqual(0, player.currencyHeld);
    }
    //===================================================================================================================//
    //===================================================================================================================//
    // UNIT TEST PART END//SPYTEST PART BEGIN
    //===================================================================================================================//
    //===================================================================================================================//

    [Test]
    public void TakeDamage_CallsSetHealthOnHealthBar()
    {
        player.TakeDamage(10);

        var spy = mockHealthBar as SpyHealthBar;

        Assert.IsTrue(spy.SetHealthCalled, "SetHealth was not called.");
        Assert.AreEqual(90, spy.LastHealthValue, "SetHealth was called with wrong value.");
    }

    [Test]
    public void UseMana_CallsSetManaOnManaBar()
    {
        player.UseMana(25);

        var spy = mockManaBar as SpyManaBar;

        Assert.IsTrue(spy.SetManaCalled, "SetMana was not called.");
        Assert.AreEqual(75, spy.LastManaValue, "SetMana was called with wrong value.");
    }

    [Test]
    public void UseStamina_CallsSetStaminaOnStaminaBar()
    {
        player.UseStamina(40);

        var spy = mockStaminaBar as SpyStaminaBar;

        Assert.IsTrue(spy.SetStaminaCalled, "SetStamina was not called.");
        Assert.AreEqual(60, spy.LastStaminaValue, "SetStamina was called with wrong value.");
    }
    [Test]
    public void UseMana_BeyondAvailable_CallsSetManaWithZero()
    {
        player.currentMana = 10;
        player.UseMana(50);

        var spy = mockManaBar as SpyManaBar;

        Assert.IsTrue(spy.SetManaCalled);
        Assert.AreEqual(0, spy.LastManaValue);
    }

    [Test]
    public void UseStamina_BeyondAvailable_CallsSetStaminaWithZero()
    {
        player.currentStamina = 10;
        player.UseStamina(50);

        var spy = mockStaminaBar as SpyStaminaBar;

        Assert.IsTrue(spy.SetStaminaCalled);
        Assert.AreEqual(0, spy.LastStaminaValue);
    }

    [Test]
    public void UseHealth_BeyondAvailable_CallsSetHealthWithZero()
    {
        player.currentHealth = 5;
        player.UseHealth(100);

        var spy = mockHealthBar as SpyHealthBar;

        Assert.IsTrue(spy.SetHealthCalled);
        Assert.AreEqual(0, spy.LastHealthValue);
    }

    [Test]
    public void UseHealth_NegativeAmount_CallsSetHealthWithIncreasedValue()
    {
        player.currentHealth = 50;
        player.UseHealth(-30); // Heal for 30

        var spy = mockHealthBar as SpyHealthBar;

        Assert.IsTrue(spy.SetHealthCalled);
        Assert.AreEqual(80, spy.LastHealthValue);
    }

    [Test]
    public void UseMana_NegativeAmount_CallsSetManaWithIncreasedValue()
    {
        player.currentMana = 40;
        player.UseMana(-20); // Simulate regen/heal

        var spy = mockManaBar as SpyManaBar;

        Assert.IsTrue(spy.SetManaCalled);
        Assert.AreEqual(60, spy.LastManaValue);
    }

    [Test]
    public void UseStamina_NegativeAmount_CallsSetStaminaWithIncreasedValue()
    {
        player.currentStamina = 70;
        player.UseStamina(-20);

        var spy = mockStaminaBar as SpyStaminaBar;

        Assert.IsTrue(spy.SetStaminaCalled);
        Assert.AreEqual(90, spy.LastStaminaValue);
    }
}