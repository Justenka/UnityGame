using UnityEngine;

[CreateAssetMenu(menuName = "Items/Material")]
public class MaterialItem : Item
{
    public int tier;
    public override bool Use(GameObject user)
    {
        return true;
    }
}