using UnityEngine;
using System.Collections.Generic;

public static class UnknownConsumableEffects
{
    private static List<System.Action<Player>> effects = new List<System.Action<Player>>()
    {
        //Stat Effects
        (player) => ModifyStat(player, StatType.Attack, 5, true),   // +5 Attack permanently
        (player) => ModifyStat(player, StatType.Defense, -3, true), // -3 Defense permanently
        (player) => ModifyStat(player, StatType.Speed, 2, true),    // +2 Speed permanently

        //Direct resource impact
        (player) => player.UseHealth(Random.Range(10, 50)),
        (player) => player.UseStamina(Random.Range(20, 40)),
        (player) => player.UseMana(Random.Range(30, 80)),

        //Trick Effects
        (player) => Debug.Log("Nothing happened... or did it?"),
        (player) => {
            Debug.Log("You feel a strange chill... but it's probably fine.");
            // Visual/audio cue placeholder here
        },
        (player) => {
            Debug.Log("Suddenly, you feel... powerful?");
            ModifyStat(player, StatType.CritChance, 100, false); // Fake bonus, not permanent
        },

        //Combo Effects
        (player) => {
            player.UseHealth(Random.Range(5, 20));
            player.RestoreStamina(50);
            Debug.Log("Energy surge! But at a cost...");
        },
        (player) => {
            player.RestoreHealth(20);
            player.UseMana(40);
            Debug.Log("Health restored at the cost of magic.");
        },

        //Dangerous
        (player) => {
            Debug.LogWarning("That tasted... cursed.");
            player.TakeDamage(50); // Bypasses defense
        }
    };

    public static void ApplyRandomEffect(Player player)
    {
        if (player == null) return;

        int index = Random.Range(0, effects.Count);
        effects[index].Invoke(player);
    }
    private static void ModifyStat(Player player, StatType type, float value, bool permanent)
    {
        if (!player.stats.ContainsKey(type)) return;

        if (permanent)
        {
            player.stats[type].baseValue += value;
            Debug.Log($"Stat change: {type} {(value >= 0 ? "+" : "")}{value} (permanent)");
        }
        else
        {
            player.stats[type].bonusValue += value;
            Debug.Log($"Stat change: {type} {(value >= 0 ? "+" : "")}{value} (temporary/fake)");
        }

        player.SendMessage("RefreshStats", SendMessageOptions.DontRequireReceiver);
    }
}
