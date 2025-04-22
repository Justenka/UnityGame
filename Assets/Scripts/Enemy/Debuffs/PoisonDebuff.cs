using UnityEngine;

public class PoisonDebuff : Debuff
{
    private float tickInterval = 1f;
    private float tickTimer;
    private float damagePerTick = 2f;

    public PoisonDebuff(float duration, float damagePerTick, float tickInterval) : base(duration, DebuffType.Poison)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
    }

    public override void Apply(Character target)
    {
        base.Apply(target);
        Debug.Log($"Poison applied to {target.gameObject.name}");
        tickTimer = 0f;
        target.ApplyUIForDebuff(debuffType);
    }

    public override void Update(Character target)
    {
        base.Update(target);
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            target.TakeDamage(damagePerTick);
            tickTimer = tickInterval;
        }
    }

    public override void Remove(Character target)
    {
        base.Remove(target);
        Debug.Log($"Poison removed from {target.gameObject.name}");
        target.RemoveUIForDebuff(debuffType);
    }
}