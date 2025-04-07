using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class HotbarSlotTests
{
    private GameObject player;
    private GameObject hotbarSlotObject;
    private HotbarSlot hotbarSlot;
    private StubConsumableItem stubConsumableItem;
    private InventoryItem inventoryItem;

    private class StubConsumableItem : ConsumableItem
    {
        public bool useCalled = false;

        public override void Use(GameObject user)
        {
            useCalled = true;
        }
    }

    // Mocked Input for testing
    private class MockInput
    {
        public static bool GetKeyDown(KeyCode key)
        {
            return key == KeyCode.Alpha1; // Simulate Alpha1 keypress for testing
        }
    }

    [SetUp]
    public void SetUp()
    {
        // Sukuriam mock objekta zaidejui
        player = new GameObject();
        player.AddComponent<Player>();

        // Sukuriam hotbar'o objekta
        hotbarSlotObject = new GameObject();
        hotbarSlot = hotbarSlotObject.AddComponent<HotbarSlot>();

        // Sukuriam stub'a naudojamui daiktui
        stubConsumableItem = ScriptableObject.CreateInstance<StubConsumableItem>();
        stubConsumableItem.cooldown = 2.0f;
        stubConsumableItem.restoreAmount = 10;

        // Sukuriam inventoriu
        inventoryItem = new GameObject().AddComponent<InventoryItem>();
        inventoryItem.InitialiseItem(stubConsumableItem);
        inventoryItem.count = 5;
        inventoryItem.transform.SetParent(hotbarSlotObject.transform);

        hotbarSlot.hotbarItem = inventoryItem;
    }


    [Test]
    public void UseHotbarItem_CallsUseMethod_WhenItemIsUsed()
    {
        // Daiktu kiekis inventoriuje
        int initialCount = hotbarSlot.hotbarItem.count;

        // Override Input.GetKeyDown to simulate Alpha1 key press ??????
        var originalGetKeyDown = typeof(Input).GetMethod("GetKeyDown", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        typeof(Input).GetMethod("GetKeyDown").Invoke(null, new object[] { KeyCode.Alpha1 });

        // Atnaujinam inventoriu
        hotbarSlot.Update();

        // Tikrinam
        Assert.IsTrue(stubConsumableItem.useCalled, "Turi buti iskviestas Use() metodas ir sunaudojamas vienas daiktas.");
        Assert.AreEqual(initialCount - 1, hotbarSlot.hotbarItem.count, "Kiekis turi pamazeti per viena panaudojus.");
    }

    [Test]
    public void UseHotbarItem_OnlyUsesConsumableItems()
    {
        // Testas daiktam kurie nera 'consumbles'. Jie neturi susinaudoti 

        Item nonConsumableItem = ScriptableObject.CreateInstance<Item>();
        hotbarSlot.hotbarItem.InitialiseItem(nonConsumableItem);

        hotbarSlot.UseHotbarItem();

        Assert.AreEqual(5, hotbarSlot.hotbarItem.count, "Non-consumable daiktai neturi susinaudoti.");
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(player);
        Object.Destroy(hotbarSlotObject);
    }
}
