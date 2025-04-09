using UnityEngine;

public class Debuff
{
    protected float duration;
    public DebuffType debuffType;
    public bool IsExpired => duration <= 0;

    public Debuff(float duration, DebuffType debuffType)
    {
        this.duration = duration;
        this.debuffType = debuffType;
    }

    public virtual void Apply(Player player) 
    {
        player.debuffUIManager.ShowDebuffIcon(debuffType);
    }
    public virtual void Remove(Player player) 
    {
        player.debuffUIManager.HideDebuffIcon(debuffType);
    }

    public virtual void Update(Player player)
    {
        duration -= Time.deltaTime; // Reduce the debuff duration

        if (duration <= 0)
        {
            Remove(player);
        }
    }
}

