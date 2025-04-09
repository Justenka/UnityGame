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

    public override void Apply(Player player)
    {
        player.debuffUIManager.ShowDebuffIcon(DebuffType.Poison);
        Debug.Log("Player poisoned!");
        tickTimer = 0f;
    }

    public override void Update(Player player)
    {
        base.Update(player);
        player.debuffUIManager.UpdateDebuffTime(DebuffType.Poison, duration);
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            player.TakeDamage(damagePerTick);
            tickTimer = tickInterval;
        }
    }

    public override void Remove(Player player)
    {
        player.debuffUIManager.HideDebuffIcon(DebuffType.Poison);
        Debug.Log("Poison removed.");
    }
}


