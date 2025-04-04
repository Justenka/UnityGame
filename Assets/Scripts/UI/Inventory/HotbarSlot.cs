using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarSlot : InventorySlot
{
    public KeyCode useKey = KeyCode.Alpha1;

    private float currentCooldown = 0f;
    private float maxCooldown = 0f;

    private InventoryItem hotbarItem;

    public Image cooldownOverlay;

    void Update()
    {
        // Check if we have a valid item in this hotbar slot
        if (transform.childCount == 1)
        {
            if (hotbarItem == null)
                hotbarItem = transform.GetChild(0).GetComponent<InventoryItem>();

            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
                if (cooldownOverlay != null)
                    cooldownOverlay.fillAmount = currentCooldown / maxCooldown;
            }
            else
            {
                if (cooldownOverlay != null)
                    cooldownOverlay.fillAmount = 0f;
            }

            if (Input.GetKeyDown(useKey) && currentCooldown <= 0)
            {
                UseHotbarItem();
            }
        }
    }

    void UseHotbarItem()
    {
        if (hotbarItem == null || hotbarItem.item == null)
            return;

        hotbarItem.item.Use(GameObject.FindGameObjectWithTag("Player"));
        hotbarItem.count--;
        hotbarItem.RefreshCount();

        if (hotbarItem.count <= 0)
        {
            Destroy(hotbarItem.gameObject);
            hotbarItem = null;
            return; // skip cooldown
        }

        // Set cooldown from item
        if (hotbarItem.item is ConsumableItem consumable)
        {
            maxCooldown = currentCooldown = consumable.cooldown;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        // Refresh reference after drop
        if (transform.childCount == 1)
        {
            hotbarItem = transform.GetChild(0).GetComponent<InventoryItem>();
        }
    }
}
