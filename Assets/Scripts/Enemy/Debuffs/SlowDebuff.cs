using UnityEngine;

public class SlowDebuff : Debuff
{
    private float slowAmount;
    private float originalSpeedMultiplier = 1f; // Renamed for clarity

    public SlowDebuff(float duration, float slowAmount) : base(duration, DebuffType.Slow)
    {
        this.slowAmount = slowAmount;
    }

    public override void Apply(Character target)
    {
        base.Apply(target);
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        SimpleEnemyAI enemyAI = target.GetComponent<SimpleEnemyAI>();

        if (playerMovement != null)
        {
            originalSpeedMultiplier = playerMovement.GetCurrentSpeedMultiplier();
            playerMovement.ModifySpeedMultiplier(originalSpeedMultiplier * (1f - slowAmount));
            Debug.Log($"Slow applied to player {target.gameObject.name}");
            target.ApplyUIForDebuff(debuffType);
        }
        else if (enemyAI != null)
        {
            originalSpeedMultiplier = enemyAI.speed; // Assuming you have a speed multiplier on the enemy AI
            enemyAI.speed *= (1f - slowAmount);
            Debug.Log($"Slow applied to enemy {target.gameObject.name}");
        }
    }

    public override void Update(Character target)
    {
        base.Update(target);
        // No specific update logic needed for the slow debuff itself,
        // the effect is applied in the Apply method and removed in Remove.
        // The base.Update handles the duration.
    }

    public override void Remove(Character target)
    {
        base.Remove(target);
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        SimpleEnemyAI enemyAI = target.GetComponent<SimpleEnemyAI>();

        if (playerMovement != null)
        {
            playerMovement.ModifySpeedMultiplier(originalSpeedMultiplier);
            Debug.Log($"Slow removed from player {target.gameObject.name}");
            target.RemoveUIForDebuff(debuffType);
        }
        else if (enemyAI != null)
        {
            enemyAI.speed = originalSpeedMultiplier;
            Debug.Log($"Slow removed from enemy {target.gameObject.name}");
        }
    }
}