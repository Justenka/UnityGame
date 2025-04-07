using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class HotbarSlotTests
{
    private GameObject player;
    private HotbarSlot hotbarSlot;
    private InventoryItem inventoryItem;
    private StubConsumableItem stubConsumableItem;

    //STUB: Simulates a consumable item and tracks usage
    private class StubConsumableItem : ConsumableItem
    {
        public bool useCalled = false;

        public override void Use(GameObject user)
        {
            useCalled = true;
        }
    }

    //STUB: Simulates a generic non-consumable item
    private class NonConsumableItem : Item
    {
    }

    //Test Helper: Create InventoryItem With Mocked UI
    private InventoryItem CreateMockInventoryItem(Item item, int count)
    {
        var go = new GameObject("InventoryItem");

        var image = go.AddComponent<Image>();

        var countTextGO = new GameObject("CountText");
        countTextGO.transform.SetParent(go.transform);
        var countText = countTextGO.AddComponent<Text>();

        var ii = go.AddComponent<InventoryItem>();
        ii.countText = countText;
        ii.count = count;

        ii.InitialiseItem(item);
        return ii;
    }

    //Test Helper: Create HotbarSlot and attach item
    private HotbarSlot CreateMockHotbarSlot(InventoryItem item)
    {
        var go = new GameObject("HotbarSlot");
        var slot = go.AddComponent<HotbarSlot>();
        item.transform.SetParent(go.transform);
        slot.hotbarItem = item;
        return slot;
    }

    [SetUp]
    public void SetUp()
    {
        player = new GameObject("Player");
        player.AddComponent<Player>();

        stubConsumableItem = ScriptableObject.CreateInstance<StubConsumableItem>();
        stubConsumableItem.cooldown = 2.0f;
        stubConsumableItem.restoreAmount = 10;

        inventoryItem = CreateMockInventoryItem(stubConsumableItem, 5);
        hotbarSlot = CreateMockHotbarSlot(inventoryItem);
    }

    [Test]
    public void UseHotbarItem_CallsUseMethod_WhenItemIsUsed()
    {
        int initialCount = inventoryItem.count;

        hotbarSlot.UseHotbarItem();

        Assert.IsTrue(stubConsumableItem.useCalled, "Use() should have been called.");
        Assert.AreEqual(initialCount - 1, inventoryItem.count, "Item count should decrease by 1.");
    }

    [Test]
    public void UseHotbarItem_OnlyUsesConsumableItems()
    {
        Item nonConsumableItem = ScriptableObject.CreateInstance<NonConsumableItem>();
        inventoryItem.InitialiseItem(nonConsumableItem);

        hotbarSlot.UseHotbarItem();

        Assert.AreEqual(5, inventoryItem.count, "Non-consumable item count should not decrease.");
    }

    [TearDown]
    public void TearDown()
    {
        if (player != null)
            Object.DestroyImmediate(player);

        if (hotbarSlot != null && hotbarSlot.gameObject != null)
            Object.DestroyImmediate(hotbarSlot.gameObject);

        if (inventoryItem != null && !ReferenceEquals(inventoryItem, null) && inventoryItem.gameObject != null)
            Object.DestroyImmediate(inventoryItem.gameObject);
    }
}
