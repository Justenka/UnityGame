using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public abstract void TakeDamage(float amount);
    public abstract void AddDebuff(Debuff newDebuff);
    public abstract void RemoveDebuff(System.Type debuffType);
    public abstract void UpdateDebuffs();
    public abstract Dictionary<System.Type, Debuff> GetActiveDebuffs();
    public virtual void ApplyUIForDebuff(DebuffType type) { }
    public virtual void UpdateUIForDebuff(DebuffType type, float currentDuration, float originalDuration) { }
    public virtual void RemoveUIForDebuff(DebuffType type) { }
}