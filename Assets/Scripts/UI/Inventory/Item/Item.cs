using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite image;
    public int price;
    public bool stackable;
    public ItemType type;

    // You can define a virtual method to override per item
    public virtual void Use(GameObject user) { }
}