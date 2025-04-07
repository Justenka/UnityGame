using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
using UnityEngine.UI;
public class TestItem : Item { }
public class NPCTest
{

    private GameObject gameObject;
    private Item Item;
    private ShopItemSlot shopItemSlot;
    private TestShopUI testShop;
    [SetUp]
    public void SetUp()
    {
        gameObject = new GameObject();
        shopItemSlot = gameObject.AddComponent<ShopItemSlot>();

        shopItemSlot.icon = new GameObject().AddComponent<Image>();
        shopItemSlot.itemNameText = new GameObject().AddComponent<TextMeshProUGUI>();
        shopItemSlot.priceText = new GameObject().AddComponent<TextMeshProUGUI>();
        shopItemSlot.buyButton = new GameObject().AddComponent<Button>();

        Item = ScriptableObject.CreateInstance<TestItem>();
        Item.itemName = "Potion";
        Item.price = 50;
        Item.image = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 4, 4), Vector2.zero);
        Item.stackable = false;
        Item.type = ItemType.All;
        Item.description = "ITEM FOR TEST";

        GameObject shopObject = new GameObject("Test Shop");
        shopObject.SetActive(false);
        testShop = shopObject.AddComponent<TestShopUI>();

        shopObject.SetActive(true);
        shopItemSlot.Setup(Item, testShop);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameObject);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void Setup_Assigns_UI_Elements_Correctly()
    {
        Assert.AreEqual(Item.image, shopItemSlot.icon.sprite);
        Assert.AreEqual("Potion", shopItemSlot.itemNameText.text);
        Assert.AreEqual("50g", shopItemSlot.priceText.text);
    }

    [Test]
    public void BuyItem_Calls_TryBuyItem_On_Shop()
    {
        shopItemSlot.buyButton.onClick.Invoke();
        Assert.IsTrue(testShop.itembought);
        Assert.AreEqual(Item, testShop.lastBoughtItem);
    }

    private class TestShopUI : ShopUI
    {
        public bool itembought = false;
        public Item lastBoughtItem;
        protected override void OnEnable()
        {
            // Paliekam tuščią – testui to nereikia
        }
        public override void TryBuyItem(Item item)
        {
            itembought = true;
            lastBoughtItem = item;

            // Jeigu tavo realus TryBuyItem tikrina inventoryManager:
            if (inventoryManager == null)
            {
                Debug.Log("Mock TryBuyItem called, no real InventoryManager present.");
                return;
            }

        }
        
    }
    
}
