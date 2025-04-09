using UnityEngine;

public class SlowDebuff : Debuff
{
    private float slowAmount;
    private float originalMultiplier = 1f;
    private PlayerMovement playerMovement;

    public SlowDebuff(float duration, float slowAmount) : base(duration, DebuffType.Slow)
    {
        this.slowAmount = slowAmount;
    }

    public override void Apply(Player player)
    {
        player.debuffUIManager.ShowDebuffIcon(DebuffType.Slow);
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            originalMultiplier = playerMovement.GetCurrentSpeedMultiplier();
            playerMovement.ModifySpeedMultiplier(originalMultiplier * (1f - slowAmount));
        }
    }
    public override void Update(Player player)
    {
        base.Update(player);
        player.debuffUIManager.UpdateDebuffTime(DebuffType.Slow, duration);
    }
    public override void Remove(Player player)
    {
        player.debuffUIManager.HideDebuffIcon(DebuffType.Slow);
        if (playerMovement != null)
        {
        playerMovement.ModifySpeedMultiplier(originalMultiplier);
        }
    }
}
