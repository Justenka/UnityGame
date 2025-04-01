using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class EnemyTests
{
    private GameObject enemyObject;
    private Enemy enemy;


    [SetUp]
    public void SetUp()
    {
        enemyObject = new GameObject("Enemy");
        enemy = enemyObject.AddComponent<Enemy>();
        enemy.rb = enemyObject.AddComponent<Rigidbody2D>();
        enemy.animator = enemyObject.AddComponent<Animator>();
        enemyObject.AddComponent<SimpleEnemyAI>();
        enemyObject.AddComponent<CapsuleCollider2D>();
        DamageNumberController.instance = null;
        enemy.currentHealth = 100;

        enemy.Start();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(enemyObject);
    }

    [Test]
    public void Enemy_Initializes_Health_Correctly()
    {
        enemy.maxHealthModifier = 2;
        enemy.Start();

        Assert.GreaterOrEqual(enemy.currentHealth, 50 * enemy.maxHealthModifier);
        Assert.LessOrEqual(enemy.currentHealth, 300 * enemy.maxHealthModifier);
    }

    [Test]
    public void Enemy_Takes_Damage()
    {
        int damage = 20;
        enemy.TakeDamage(damage, Vector2.zero, 0, false);
        Assert.AreEqual(80, enemy.currentHealth, "Health should be reduced after taking damage.");
    }

    [Test]
    public void Enemy_Dies_When_Health_Reaches_Zero()
    {
        enemy.currentHealth = 50;
        enemy.TakeDamage(50, Vector2.zero, 0, false);
        Assert.IsTrue(enemy.isDead, "Enemy should be dead when health reaches zero.");
    }

    [UnityTest]
    public IEnumerator Enemy_Invincibility_Wears_Off()
    {
        enemy.StartInvincibility();
        Assert.IsTrue(enemy.isInvincible, "Enemy should start as invincible.");
        yield return new WaitForSeconds(enemy.invincibilityDuration + 0.1f);
        Assert.IsFalse(enemy.isInvincible, "Enemy should no longer be invincible after duration.");
    }

    [Test]
    public void Enemy_Apply_Knockback_Sets_Velocity()
    {
        enemy.currentHealth = 100;
        enemy.TakeDamage(10, Vector2.left, 5f, true);
        Assert.AreNotEqual(Vector2.zero, enemy.rb.linearVelocity);
    }
}
