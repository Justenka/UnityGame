using UnityEngine;

public class BurnDebuff : Debuff
{
    private float tickInterval = 1f;
    private float tickTimer;
    private float damagePerTick = 5f;

    public BurnDebuff(float duration, float damagePerTick, float tickInterval) : base(duration, DebuffType.Burn)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
    }

    public override void Apply(Character target)
    {
        base.Apply(target);
        Debug.Log($"Burn started on {target.gameObject.name}");
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
        Debug.Log($"Burn ended on {target.gameObject.name}");
        target.RemoveUIForDebuff(debuffType);
    }
}