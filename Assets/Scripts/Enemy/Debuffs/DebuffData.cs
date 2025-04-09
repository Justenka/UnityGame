using UnityEngine;

[System.Serializable]
public class DebuffData
{
    public DebuffType debuffType;
    public float duration = 5f;
    public float damagePerTick = 1f;      // Used for Poison/Burn
    public float tickInterval = 1f;       // Used for Poison/Burn
    public float slowAmount = 0.5f;       // Used for Slow (e.g., 0.5 = 50% slower)
}