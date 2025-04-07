using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarSlot : InventorySlot
{
    public KeyCode useKey = KeyCode.Alpha1;
    public InventoryItem hotbarItem;

    public Image cooldownOverlay;

    private float currentCooldown = 0f;
    private float maxCooldown = 0f;
    private bool isOnCooldown = false;

    void Start()
    {
        if (cooldownOverlay != null)
            cooldownOverlay.gameObject.SetActive(false);
    }
    public void Update()
    {
        hotbarItem = GetComponentInChildren<InventoryItem>();

        if (hotbarItem != null && hotbarItem.item is ConsumableItem)
        {
            if (!isOnCooldown && Input.GetKeyDown(useKey))
            {
                UseHotbarItem();
            }
        }

        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;

            if (cooldownOverlay != null)
            {
                cooldownOverlay.gameObject.SetActive(true);
                cooldownOverlay.fillAmount = currentCooldown / maxCooldown;
            }

            if (currentCooldown <= 0f)
            {
                isOnCooldown = false;
                if (cooldownOverlay != null)
                    cooldownOverlay.gameObject.SetActive(false);
            }
        }
    }

    public void UseHotbarItem()
    {
        if (hotbarItem == null || hotbarItem.item == null)
            return;

        if (!(hotbarItem.item is ConsumableItem consumable))
        {
            Debug.LogWarning("Only consumable items can be used from the hotbar.");
            return;
        }

        consumable.Use(GameObject.FindGameObjectWithTag("Player"));

        hotbarItem.count--;
        hotbarItem.RefreshCount();

        if (hotbarItem.count <= 0)
        {
            Destroy(hotbarItem.gameObject);
            hotbarItem = null;
            return;
        }

        // Start cooldown here in HotbarSlot, not on InventoryItem
        maxCooldown = currentCooldown = consumable.cooldown;
        isOnCooldown = true;

        if (cooldownOverlay != null)
        {
            cooldownOverlay.gameObject.SetActive(true);
            cooldownOverlay.fillAmount = 1f;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        hotbarItem = GetComponentInChildren<InventoryItem>();
    }
}