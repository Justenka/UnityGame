using NUnit.Framework;
using UnityEngine;

public class ExperienceLevelTests
{
    private ExperienceLevelController expController;

    [SetUp]
    public void Setup()
    {
        GameObject expControllerObject = new GameObject("ExperienceLevelController");
        expController = expControllerObject.AddComponent<ExperienceLevelController>();

        expController.currentExperience = 0;
        expController.currentLevel = 1;
        expController.experienceToNextLevel = 100;
        expController.levelMultiplier = 1.2f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(expController.gameObject);
    }

    [Test]
    public void GetExp_IncreasesExperience()
    {
        expController.GetExp(50);
        Assert.AreEqual(50, expController.currentExperience);
    }

    [Test]
    public void GetExp_TriggersLevelUp()
    {
        expController.GetExp(150);
        Assert.AreEqual(2, expController.currentLevel);
        Assert.AreEqual(50, expController.currentExperience);
    }

    [Test]
    public void GetExp_ExactLevelUp()
    {
        expController.GetExp(100);
        Assert.AreEqual(2, expController.currentLevel);
        Assert.AreEqual(0, expController.currentExperience);
    }

    [Test]
    public void GetExp_MultipleLevelUps()
    {
        expController.GetExp(250);
        Assert.AreEqual(3, expController.currentLevel);
        Assert.AreEqual(30, expController.currentExperience);
    }

    [Test]
    public void GetExp_LevelUpEventIsTriggered()
    {
        int newLevel = -1;
        expController.OnLevelUp += (level) => newLevel = level;

        expController.GetExp(100);
        Assert.AreEqual(2, newLevel);
    }

    [Test]
    public void GetExp_NoLevelUp_WhenNotEnoughXP()
    {
        expController.GetExp(99);
        Assert.AreEqual(1, expController.currentLevel);
        Assert.AreEqual(99, expController.currentExperience);
    }

    [Test]
    public void GetExp_LargeXPAmount()
    {
        expController.GetExp(1000);
        Assert.Greater(expController.currentLevel, 3);
    }
}