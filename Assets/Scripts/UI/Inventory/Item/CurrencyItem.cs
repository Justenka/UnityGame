using UnityEngine;

[CreateAssetMenu(menuName = "Items/Currency")]
public class CurrencyItem : Item
{
    public int amount;

    public override void Use(GameObject user)
    {
        // Add to player wallet
    }
}