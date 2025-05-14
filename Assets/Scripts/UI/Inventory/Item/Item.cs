using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public StatType statType;
    public float baseValue;
    public float value;
}

public abstract class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite image;
    public int price;
    public bool stackable;
    public ItemType type;
    public ItemRarity rarity;

    public List<StatModifier> statModifiers = new();

    public virtual bool Use(GameObject user) => false;

    public virtual bool IsSameItem(Item other)
    {
        return other != null && itemName == other.itemName && GetType() == other.GetType();
    }

    public virtual Item Clone()
    {
        return Instantiate(this);
    }
}