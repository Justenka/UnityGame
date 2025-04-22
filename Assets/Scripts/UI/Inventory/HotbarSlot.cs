using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarSlot : InventorySlot
{
    public KeyCode useKey = KeyCode.Alpha1;
    public Image cooldownOverlay;

    private InventoryItem hotbarItem;

    private float currentCooldown = 0f;
    private float maxCooldown = 0f;
    private bool isOnCooldown = false;

    private void Start()
    {
        if (cooldownOverlay != null)
            cooldownOverlay.gameObject.SetActive(false);
    }

    private void Update()
    {
        hotbarItem = GetComponentInChildren<InventoryItem>();

        if (hotbarItem != null && hotbarItem.item is ConsumableItem && !isOnCooldown)
        {
            if (Input.GetKeyDown(useKey))
            {
                TryUseHotbarItem();
            }
        }

        HandleCooldownVisuals();
    }

    void TryUseHotbarItem()
    {
        if (hotbarItem == null || !(hotbarItem.item is ConsumableItem consumable))
            return;

        bool used = consumable.Use(GameObject.FindGameObjectWithTag("Player"));
        if (!used) return;

        // ✅ Reduce the count directly
        hotbarItem.count--;
        if (hotbarItem.count <= 0)
        {
            Destroy(hotbarItem.gameObject);
            hotbarItem = null;
        }
        else
        {
            hotbarItem.RefreshCount();
        }

        StartCooldown(consumable.cooldown);
    }

    void StartCooldown(float cooldown)
    {
        maxCooldown = currentCooldown = cooldown;
        isOnCooldown = true;

        if (cooldownOverlay != null)
        {
            cooldownOverlay.gameObject.SetActive(true);
            cooldownOverlay.fillAmount = 1f;
        }
    }

    void HandleCooldownVisuals()
    {
        if (!isOnCooldown) return;

        currentCooldown -= Time.deltaTime;

        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = currentCooldown / maxCooldown;
        }

        if (currentCooldown <= 0f)
        {
            isOnCooldown = false;
            if (cooldownOverlay != null)
                cooldownOverlay.gameObject.SetActive(false);
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        InventoryItem droppedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (droppedItem != null)
        {
            // Remove existing item in the hotbar slot
            InventoryItem existingItem = GetComponentInChildren<InventoryItem>();
            if (existingItem != null && existingItem != droppedItem)
            {
                Destroy(existingItem.gameObject);
            }

            // ✅ Move item into this slot
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;
            droppedItem.transform.localScale = Vector3.one;

            hotbarItem = droppedItem;
        }
    }
}
