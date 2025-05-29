using UnityEngine;
using System.Collections;

public class Debuff
{
    protected float duration;
    public DebuffType debuffType;
    public bool IsExpired => duration <= 0;
    protected float originalDuration;

    public Debuff(float duration, DebuffType debuffType)
    {
        this.duration = duration;
        this.debuffType = debuffType;
        this.originalDuration = duration;
    }

    public virtual void Apply(Character target)
    {
        Debug.Log($"Debuff {debuffType} applied to {target.gameObject.name}");
        target.UpdateUIForDebuff(debuffType, duration, originalDuration);
    }

    public virtual void Remove(Character target)
    {
        Debug.Log($"Debuff {debuffType} removed from {target.gameObject.name}");
        target.UpdateUIForDebuff(debuffType, 0f, originalDuration);
    }

    public virtual void Update(Character target)
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Remove(target);
        }
        else
        {
            target.UpdateUIForDebuff(debuffType, duration, originalDuration);
        }
    }
}