using UnityEngine;

[CreateAssetMenu(menuName = "Items/Material")]
public class MaterialItem : Item
{
    public override bool Use(GameObject user)
    {
        return true;
    }
}