using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorItem : Item
{
    public ArmorType armorType;

    public override bool Use(GameObject user)
    {
        return true;
    }
}