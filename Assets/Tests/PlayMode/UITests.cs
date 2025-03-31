using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class UITests
{
    private HealthBar healthBar;
    private ManaBar manaBar;
    private StaminaBar staminaBar;

    [SetUp]
    public void Setup()
    {
        GameObject healthBarObject = new GameObject("HealthBarObject");
        healthBar = healthBarObject.AddComponent<HealthBar>();
        healthBar.healthSlider = healthBarObject.AddComponent<Slider>();

        GameObject manaBarObject = new GameObject("ManaBarObject");
        manaBar = manaBarObject.AddComponent<ManaBar>();
        manaBar.manaSlider = manaBarObject.AddComponent<Slider>();

        GameObject staminaBarObject = new GameObject("StaminaBarObject");
        staminaBar = staminaBarObject.AddComponent<StaminaBar>();
        staminaBar.staminaSlider = staminaBarObject.AddComponent<Slider>();

        InitializeSlider(healthBar.healthSlider);
        InitializeSlider(manaBar.manaSlider);
        InitializeSlider(staminaBar.staminaSlider);
    }

    private void InitializeSlider(Slider slider)
    {
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 50;
    }

    [Test]
    public void SetHealth_UpdatesHealthValue()
    {
        healthBar.SetHealth(75);
        Assert.AreEqual(75, healthBar.healthSlider.value, "Health value did not update correctly.");    // Trecias argumentas isveda klaida jeigu nesutampa rezultatai.
    }

    [Test]
    public void SetMaxHealth_SetsCorrectMaxValue()
    {
        healthBar.SetMaxHealth(100);
        Assert.AreEqual(100, healthBar.healthSlider.maxValue, "Health max value did not update.");
    }

    [Test]
    public void SetMana_UpdatesManaValue()
    {
        manaBar.SetMana(50);
        Assert.AreEqual(50, manaBar.manaSlider.value, "Mana value did not update correctly.");
    }

    [Test]
    public void SetMaxMana_SetsCorrectMaxValue()
    {
        manaBar.SetMaxMana(80);
        Assert.AreEqual(80, manaBar.manaSlider.maxValue, "Mana max value did not update.");
    }

    [Test]
    public void SetStamina_UpdatesStaminaValue()
    {
        staminaBar.SetStamina(30);
        Assert.AreEqual(30, staminaBar.staminaSlider.value, "Stamina value did not update correctly.");
    }

    [Test]
    public void SetMaxStamina_SetsCorrectMaxValue()
    {
        staminaBar.SetMaxStamina(90);
        Assert.AreEqual(90, staminaBar.staminaSlider.maxValue, "Stamina max value did not update.");
    }
}