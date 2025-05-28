using UnityEngine;

public class StunDebuff : Debuff
{
    private float originalSpeed = 1f; 

    public StunDebuff(float duration) : base(duration, DebuffType.Stun)
    {
    }

    public override void Apply(Character target)
    {
        base.Apply(target);
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        SimpleEnemyAI enemyAI = target.GetComponent<SimpleEnemyAI>();

        if (playerMovement != null)
        {
            originalSpeed = playerMovement.GetCurrentSpeedMultiplier();
            playerMovement.ModifySpeedMultiplier(0);
            target.ApplyUIForDebuff(debuffType);
        }
        else if (enemyAI != null)
        {
            originalSpeed = enemyAI.speed;
            enemyAI.speed *= 0;
        }
    }

    public override void Update(Character target)
    {
        base.Update(target);
    }

    public override void Remove(Character target)
    {
        base.Remove(target);
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        SimpleEnemyAI enemyAI = target.GetComponent<SimpleEnemyAI>();

        if (playerMovement != null)
        {
            playerMovement.ModifySpeedMultiplier(originalSpeed);
            target.RemoveUIForDebuff(debuffType);
        }
        else if (enemyAI != null)
        {
            enemyAI.speed = originalSpeed;
        }
    }
}