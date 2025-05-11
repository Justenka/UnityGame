using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private Item item;
    private ShopUI shop;

    public void Setup(Item newItem, ShopUI shopUI)
    {
        item = newItem;
        shop = shopUI;

        icon.sprite = item.image;
        itemNameText.text = item.itemName;
        priceText.text = item.price + "g";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

        var tooltipTrigger = icon.GetComponent<TooltipTrigger>();
        if (tooltipTrigger != null)
            tooltipTrigger.SetItem(item);
    }

    private void BuyItem()
    {
        shop.TryBuyItem(item);
    }
}
