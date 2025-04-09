using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class UnknownConsumableEffects
{
    private static List<System.Action<Player>> effects = new List<System.Action<Player>>()
    {
        //Stat Effects
        (player) => ModifyStat(player, StatType.Attack, 10, true, "You feel a surge of strength in your arms... [+10 Attack]"),
        (player) => ModifyStat(player, StatType.Attack, -5, true, "Your grip weakens. Everything feels heavier... [-5 Attack]"),
        (player) => ModifyStat(player, StatType.Defense, 8, true, "A mysterious force wraps around you. [+8 Defense]"),
        (player) => ModifyStat(player, StatType.Defense, -4, true, "Your skin itches... and feels more vulnerable. [-4 Defense]"),
        (player) => ModifyStat(player, StatType.Speed, 3, true, "You feel lighter, faster, like wind on your feet. [+3 Speed]"),
        (player) => ModifyStat(player, StatType.Speed, -2, true, "Your legs feel heavy and sluggish. [-2 Speed]"),
        (player) => ModifyStat(player, StatType.CritChance, 15, true, "Your eyes sharpen. You sense weak spots in everything. [+15% Crit Chance]"),
        (player) => ModifyStat(player, StatType.CritChance, -10, true, "You feel dull... less lucky somehow. [-10% Crit Chance]"),
        (player) => ModifyStat(player, StatType.Health, 25, true, "A warm light fills you. Your body feels hardier. [+25 Max Health]"),
        (player) => ModifyStat(player, StatType.Health, -20, true, "You feel like something inside you broke. [-20 Max Health]"),
        (player) => ModifyStat(player, StatType.Stamina, 20, true, "Your muscles twitch with untapped energy. [+20 Max Stamina]"),
        (player) => ModifyStat(player, StatType.Stamina, -15, true, "Breathing suddenly feels harder... [-15 Max Stamina]"),
        (player) => ModifyStat(player, StatType.Mana, 30, true, "Your thoughts buzz with arcane potential. [+30 Max Mana]"),
        (player) => ModifyStat(player, StatType.Mana, -25, true, "Your mind feels foggy. Magic slips from your grasp. [-25 Max Mana]"),


        //Direct resource impact
        (player) => player.UseHealth(Random.Range(10, 50)),
        (player) => player.UseStamina(Random.Range(20, 40)),
        (player) => player.UseMana(Random.Range(30, 80)),

        //Trick Effects
        (player) => Debug.Log("Nothing happened... or did it?"),
        (player) => {
            Debug.Log("You feel a strange chill... but it's probably fine.");

            PlayerAudioManager audioManager = player.GetComponent<PlayerAudioManager>();
            if (audioManager != null)
            {
            audioManager.PlayStrangeEffect();
            }
        },
        (player) => {
            ModifyStat(player, StatType.CritChance, 100, false, "Suddenly, you feel... powerful?"); // Fake bonus, not permanent
        },

        //Combo Effects
        (player) => {
            player.UseHealth(Random.Range(5, 20));
            player.RestoreStamina(50);
            Debug.Log("Energy surge! But at a cost...");
        },
        (player) =>
        {
            player.RestoreHealth(20);
            player.UseMana(40);
            Debug.Log("Health restored at the cost of magic.");
        },
        (player) =>
        {
            ModifyStat(player, StatType.Attack, 5, true, "You grow stronger...");
            ModifyStat(player, StatType.Defense, -3, true, "...but at the cost of your defenses.");
        },

        (player) =>
        {
            ModifyStat(player, StatType.Speed, 2, true, "You feel faster...");
            ModifyStat(player, StatType.CritChance, -5, true, "...but less precise.");
        },

        //Dangerous
        (player) =>
        {
            Debug.LogWarning("That tasted... cursed.");
            player.TakeDamage(Random.Range(10, 50));
        },
        (player) =>
        {
            Debug.LogWarning("Your heart skips a beat. Then another. Then—");
            player.TakeDamage(Random.Range(50, 80));
        },
        (player) =>
        {
            Debug.Log("You feel your blood boil. Vitality drains from you.");
            player.TakeDamage(Random.Range(20, 40));
        },

        //Temporary Buffs
        (player) => ApplyTemporaryStatChange(player, StatType.Speed, 10, 10f, "You feel like lightning! [+10 Speed for 10s]"),
        (player) => ApplyTemporaryStatChange(player, StatType.Attack, 20, 5f, "Rage courses through your veins! [+20 Attack for 5s]"),
        (player) => ApplyTemporaryStatChange(player, StatType.Defense, 15, 8f, "An invisible shield surrounds you. [+15 Defense for 8s]"),
        (player) => ApplyTemporaryStatChange(player, StatType.CritChance, 50, 6f, "Your instincts sharpen. [+50% Crit Chance for 6s]"),
        (player) => ApplyTemporaryStatChange(player, StatType.Stamina, 30, 10f, "You breathe deeper, move quicker. [+30 Max Stamina for 10s]"),
        (player) => ApplyTemporaryStatChange(player, StatType.Mana, -50, 7f, "A magical drought overwhelms you. [-50 Max Mana for 7s]"),

    };

public static void ApplyRandomEffect(Player player)
    {
        if (player == null) return;

        int index = Random.Range(0, effects.Count);
        effects[index].Invoke(player);
    }
    private static void ModifyStat(Player player, StatType type, float value, bool permanent, string flavorText)
    {
        if (!player.stats.ContainsKey(type)) return;

        if (permanent)
        {
            player.stats[type].baseValue += value;
        }
        else
        {
            player.stats[type].bonusValue += value;
        }

        player.SendMessage("RefreshStats", SendMessageOptions.DontRequireReceiver);
        Debug.Log(flavorText);
    }
    private static void ApplyTemporaryStatChange(Player player, StatType stat, float value, float duration, string flavorText)
    {
        if (!player.stats.ContainsKey(stat)) return;

        Debug.Log(flavorText);
        player.StartCoroutine(TemporaryStatRoutine(player, stat, value, duration));
    }

    private static IEnumerator TemporaryStatRoutine(Player player, StatType stat, float value, float duration)
    {
        var statValue = player.stats[stat];
        statValue.bonusValue += value;
        player.SendMessage("RefreshStats", SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(duration);

        statValue.bonusValue -= value;
        player.SendMessage("RefreshStats", SendMessageOptions.DontRequireReceiver);
        Debug.Log($"The effect wears off. [{stat} reverted]");
    }

}
