using UnityEngine;

[CreateAssetMenu(menuName = "Items/Currency")]
public class CurrencyItem : Item
{
    public int amount;

    public override bool Use(GameObject user)
    {
        // Add to player wallet
        return true;
    }
}