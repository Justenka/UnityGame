using UnityEngine;

public class BurnDebuff : Debuff
{
    private float tickInterval = 1f;
    private float tickTimer;
    private float damagePerTick = 5f;

    public BurnDebuff(float duration, float damagePerTick, float tickInterval) : base(duration, DebuffType.Poison)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
    }

    public override void Apply(Player player)
    {
        Debug.Log("Burn started");
        player.debuffUIManager.ShowDebuffIcon(DebuffType.Burn);
        // Optional: set fire effect, color tint, etc.
        tickTimer = 0f;
    }

    public override void Update(Player player)
    {
        base.Update(player);
        player.debuffUIManager.UpdateDebuffTime(DebuffType.Burn, duration);
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            player.TakeDamage(damagePerTick);
            tickTimer = tickInterval;
        }
    }

    public override void Remove(Player player)
    {
        player.debuffUIManager.HideDebuffIcon(DebuffType.Burn);
        Debug.Log("Burn ended");
    }
}

