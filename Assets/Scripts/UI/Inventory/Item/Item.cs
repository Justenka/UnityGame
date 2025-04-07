using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public StatType statType;
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

    public virtual List<StatModifier> statModifiers { get; } = new();

    public virtual void Use(GameObject user) { }
}