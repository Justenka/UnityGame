using UnityEngine;

[System.Serializable]
public class DebuffData
{
    public DebuffType debuffType;
    public float duration;
    public float damagePerTick;      // Used for Poison/Burn
    public float tickInterval;       // Used for Poison/Burn
    public float slowAmount;       // Used for Slow (e.g., 0.5 = 50% slower)
}