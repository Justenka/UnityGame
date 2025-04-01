using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStatsUI : MonoBehaviour
{
    public Player player;
    public StatDisplayEntry displayEntryPrefab;
    public Transform statContainer;

    private Dictionary<StatType, StatDisplayEntry> statEntries = new();

    void Start()
    {
        foreach (var stat in player.stats)
        {
            StatDisplayEntry entry = Instantiate(displayEntryPrefab, statContainer);
            entry.SetStat(stat.Key, stat.Value.Total);
            statEntries.Add(stat.Key, entry);
        }
    }

    void Update()
    {
        foreach (var kvp in player.stats)
        {
            statEntries[kvp.Key].UpdateValue(kvp.Value.Total);
        }
    }
}
