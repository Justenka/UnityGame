using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;
using System.Collections;
using System.Collections.Generic;

// === SPIES ===
public class SpyHealthBar : HealthBar
{
    public bool SetHealthCalled;
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
    public bool SetManaCalled;
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
    public bool SetStaminaCalled;
    public float LastStaminaValue;

    public override void SetStamina(float value)
    {
        SetStaminaCalled = true;
        LastStaminaValue = value;
        base.SetStamina(value);
    }
}

public class SpyEquipmentManager : EquipmentManager
{
    public bool UseWeaponCalled;
    public GameObject UsedBy;

    public override void UseWeapon(GameObject user)
    {
        UseWeaponCalled = true;
        UsedBy = user;
    }
}

// === STUB ITEM FOR TESTING ===
[CreateAssetMenu(fileName = "StubItem", menuName = "Items/StubItem")]
public class StubItem : Item
{
    public override List<StatModifier> statModifiers => new()
    {
        new StatModifier { statType = StatType.Health, value = 25 },
        new StatModifier { statType = StatType.Attack, value = 10 }
    };
}

// === TESTS ===
public class PlayerTests
{
    private GameObject playerGameObject;
    private Player player;
    private SpyHealthBar spyHealthBar;
    private SpyManaBar spyManaBar;
    private SpyStaminaBar spyStaminaBar;

    [SetUp]
    public void Setup()
    {
        playerGameObject = new GameObject("Player");
        player = playerGameObject.AddComponent<Player>();

        spyHealthBar = new GameObject("HealthBar").AddComponent<SpyHealthBar>();
        spyHealthBar.healthSlider = spyHealthBar.gameObject.AddComponent<Slider>();
        player.healthBar = spyHealthBar;

        spyManaBar = new GameObject("ManaBar").AddComponent<SpyManaBar>();
        spyManaBar.manaSlider = spyManaBar.gameObject.AddComponent<Slider>();
        player.manaBar = spyManaBar;

        spyStaminaBar = new GameObject("StaminaBar").AddComponent<SpyStaminaBar>();
        spyStaminaBar.staminaSlider = spyStaminaBar.gameObject.AddComponent<Slider>();
        player.staminaBar = spyStaminaBar;

        var userNameObject = new GameObject("UserName");
        player.userName = userNameObject.AddComponent<TextMeshProUGUI>();

        playerGameObject.AddComponent<SpyEquipmentManager>();

        player.Awake();
        player.Start();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerGameObject);
        Object.DestroyImmediate(spyHealthBar.gameObject);
        Object.DestroyImmediate(spyManaBar.gameObject);
        Object.DestroyImmediate(spyStaminaBar.gameObject);
        PlayerPrefs.DeleteKey("PlayerUsername");
    }

    // === STATS / INIT ===
    [Test]
    public void Stats_Initialize_To_Max()
    {
        Assert.AreEqual(player.stats[StatType.Health].Total, player.stats[StatType.Health].currentValue);
        Assert.AreEqual(player.stats[StatType.Mana].Total, player.stats[StatType.Mana].currentValue);
        Assert.AreEqual(player.stats[StatType.Stamina].Total, player.stats[StatType.Stamina].currentValue);
    }

    // === DAMAGE / DEATH ===
    [Test]
    public void TakeDamage_Reduces_Health_With_Defense()
    {
        player.stats[StatType.Defense].baseValue = 5;
        player.stats[StatType.Health].currentValue = 100;
        player.TakeDamage(25);
        Assert.AreEqual(80f, player.stats[StatType.Health].currentValue);
    }

    [UnityTest]
    public IEnumerator TakeDamage_Just_Before_Death_Does_Kill_Player()
    {
        player.stats[StatType.Health].currentValue = 100;
        player.stats[StatType.Defense].baseValue = 0;

        player.TakeDamage(99.5f);
        yield return null;

        Assert.IsTrue(player == null || player.gameObject == null);
    }

    [UnityTest]
    public IEnumerator TakeDamage_Reaches_Zero_Health_Kills_Player()
    {
        player.stats[StatType.Health].currentValue = 100;
        player.stats[StatType.Defense].baseValue = 0;
        player.TakeDamage(100f);
        yield return new WaitForSeconds(1.5f);
        Assert.IsTrue(player == null || player.gameObject == null);
    }

    [UnityTest]
    public IEnumerator Invincibility_Activates_And_Expires()
    {
        player.TakeDamage(10);
        Assert.IsTrue(player.isInvincible);
        yield return new WaitForSeconds(player.invincibilityDuration + 0.1f);
        Assert.IsFalse(player.isInvincible);
    }

    // === MANA ===
    [Test]
    public void UseMana_Decreases_Value_And_Calls_SetMana()
    {
        player.UseMana(40);
        Assert.AreEqual(60, player.stats[StatType.Mana].currentValue);
        Assert.IsTrue(spyManaBar.SetManaCalled);
        Assert.AreEqual(60, spyManaBar.LastManaValue);
    }

    [UnityTest]
    public IEnumerator UseMana_Regenerates_After_Delay()
    {
        player.UseMana(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.stats[StatType.Mana].currentValue, 50);
    }

    [Test]
    public void RestoreMana_Increases_Mana_UpTo_Max()
    {
        player.stats[StatType.Mana].currentValue = 60;
        player.RestoreMana(25);
        Assert.AreEqual(85, player.stats[StatType.Mana].currentValue);
        Assert.AreEqual(85, spyManaBar.LastManaValue);
    }

    [Test]
    public void RestoreMana_DoesNot_Exceed_Max()
    {
        player.stats[StatType.Mana].currentValue = 90;
        player.RestoreMana(50);
        Assert.AreEqual(100, player.stats[StatType.Mana].currentValue);
        Assert.AreEqual(100, spyManaBar.LastManaValue);
    }

    // === STAMINA ===
    [Test]
    public void UseStamina_Decreases_Value_And_Calls_SetStamina()
    {
        player.UseStamina(30);
        Assert.AreEqual(70, player.stats[StatType.Stamina].currentValue);
        Assert.IsTrue(spyStaminaBar.SetStaminaCalled);
        Assert.AreEqual(70, spyStaminaBar.LastStaminaValue);
    }

    [UnityTest]
    public IEnumerator UseStamina_Regenerates_After_Delay()
    {
        player.UseStamina(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.stats[StatType.Stamina].currentValue, 50);
    }

    [Test]
    public void RestoreStamina_Increases_Stamina_UpTo_Max()
    {
        player.stats[StatType.Stamina].currentValue = 40;
        player.RestoreStamina(30);
        Assert.AreEqual(70, player.stats[StatType.Stamina].currentValue);
        Assert.AreEqual(70, spyStaminaBar.LastStaminaValue);
    }

    [Test]
    public void RestoreStamina_DoesNot_Exceed_Max()
    {
        player.stats[StatType.Stamina].currentValue = 95;
        player.RestoreStamina(20);
        Assert.AreEqual(100, player.stats[StatType.Stamina].currentValue);
        Assert.AreEqual(100, spyStaminaBar.LastStaminaValue);
    }

    // === HEALTH ===
    [Test]
    public void UseHealth_Decreases_Value_And_Calls_SetHealth()
    {
        player.UseHealth(25);
        Assert.AreEqual(75, player.stats[StatType.Health].currentValue);
        Assert.IsTrue(spyHealthBar.SetHealthCalled);
        Assert.AreEqual(75, spyHealthBar.LastHealthValue);
    }

    [UnityTest]
    public IEnumerator UseHealth_Regenerates_After_Delay()
    {
        player.UseHealth(50);
        yield return new WaitForSeconds(1.5f);
        Assert.Greater(player.stats[StatType.Health].currentValue, 50);
    }

    [Test]
    public void RestoreHealth_Increases_Health_UpTo_Max()
    {
        player.stats[StatType.Health].currentValue = 50;
        player.RestoreHealth(30);
        Assert.AreEqual(80, player.stats[StatType.Health].currentValue);
        Assert.AreEqual(80, spyHealthBar.LastHealthValue);
    }

    [Test]
    public void RestoreHealth_DoesNot_Exceed_Max()
    {
        player.stats[StatType.Health].currentValue = 95;
        player.RestoreHealth(20);
        Assert.AreEqual(100, player.stats[StatType.Health].currentValue);
        Assert.AreEqual(100, spyHealthBar.LastHealthValue);
    }

    // === CURRENCY ===
    [Test]
    public void Currency_Adding_And_Removal()
    {
        player.GetCurrency(100);
        Assert.AreEqual(100, player.currencyHeld);

        player.RemoveCurrency(40);
        Assert.AreEqual(60, player.currencyHeld);

        player.RemoveCurrency(100);
        Assert.AreEqual(0, player.currencyHeld);
    }

    // === ITEM STATS (STUB ITEM) ===
    [Test]
    public void ApplyItemStats_Updates_StatTotals()
    {
        var stubItem = ScriptableObject.CreateInstance<StubItem>();
        float originalHealth = player.stats[StatType.Health].Total;
        float originalAttack = player.stats[StatType.Attack].Total;

        player.ApplyItemStats(stubItem);

        Assert.AreEqual(originalHealth + 25, player.stats[StatType.Health].Total);
        Assert.AreEqual(originalAttack + 10, player.stats[StatType.Attack].Total);
    }

    [Test]
    public void RemoveItemStats_Reverts_StatTotals()
    {
        var stubItem = ScriptableObject.CreateInstance<StubItem>();
        player.ApplyItemStats(stubItem);

        float midHealth = player.stats[StatType.Health].Total;
        player.RemoveItemStats(stubItem);
        float endHealth = player.stats[StatType.Health].Total;

        Assert.AreEqual(midHealth - 25, endHealth);
    }

    // === EQUIPMENT USE (SPY MANAGER) ===
    [Test]
    public void UseWeapon_Triggers_EquipmentManager()
    {
        var manager = player.GetComponent<SpyEquipmentManager>();
        Assert.IsNotNull(manager);

        // Simulate click
        player.Update();
        manager.UseWeapon(playerGameObject);

        Assert.IsTrue(manager.UseWeaponCalled);
        Assert.AreEqual(playerGameObject, manager.UsedBy);
    }
}
