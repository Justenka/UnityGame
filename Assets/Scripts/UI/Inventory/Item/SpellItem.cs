using UnityEngine;

[CreateAssetMenu(menuName = "Items/Spell")]
public class SpellItem : Item
{
    public float cooldown = 1f;
    public AbilityType abilityType;
    public enum AbilityType
    {
        AoE,
        Radial,
        SummonCompanion,
        TimeSlow,
        Armageddon
    }

    public override bool Use(GameObject user)
    {
        Player player = user.GetComponent<Player>();
        if (player == null) return false;
        {
            switch (abilityType)
            {
                case AbilityType.AoE:
                    player.GetComponent<AOEAbility>().TriggerAoE(user);
                    break;
                case AbilityType.Radial:
                    player.GetComponent<RadialAttack>().TriggerAttack(user);
                    break;
                case AbilityType.SummonCompanion:
                    player.GetComponent<SummonCompanion>().Summon(user);
                    break;
                case AbilityType.TimeSlow:
                    player.GetComponent<TimeSlowAbility>().TimeSlow(user);
                    break;
                case AbilityType.Armageddon:
                    player.GetComponent<Armagedon>().Activate();
                    break;
            }
            return true;
        }
    }
}
