using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public EquipmentManager equipmentManager;
    public ArmorType acceptedArmorType;

    public override void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        // ✅ Validate dragged item type
        if (acceptedType != ItemType.All && draggedItem.item.type != acceptedType)
        {
            Debug.Log($"Rejected: {draggedItem.item.itemName} is type {draggedItem.item.type}, expected {acceptedType}");
            return;
        }

        // ✅ Armor type check
        if (draggedItem.item is ArmorItem armorItem && acceptedType == ItemType.Armor)
        {
            if (armorItem.armorType != acceptedArmorType)
            {
                Debug.Log($"Rejected: {armorItem.itemName} is {armorItem.armorType}, expected {acceptedArmorType}");
                return;
            }
        }

        // Find the correct container
        Transform container = transform.Find("ItemContainer");
        Transform targetParent = container != null ? container : transform;

        InventoryItem existingItem = targetParent.GetComponentInChildren<InventoryItem>();

        if (existingItem != null && existingItem != draggedItem)
        {
            // Check if existing item can go into the dragged item's previous slot
            Transform oldContainer = draggedItem.parentAfterDrag.Find("ItemContainer");
            Transform oldTarget = oldContainer != null ? oldContainer : draggedItem.parentAfterDrag;

            InventorySlot oldSlot = oldTarget.GetComponentInParent<InventorySlot>();

            if (oldSlot != null)
            {
                // ❗ Validate existing item type for old slot
                if (oldSlot.acceptedType != ItemType.All && existingItem.item.type != oldSlot.acceptedType)
                {
                    Debug.Log("Rejected swap: type mismatch for old slot");
                    return;
                }

                // ❗ Validate armor type
                if (existingItem.item is ArmorItem oldArmor && oldSlot is EquipmentSlot oldEquipSlot &&
                    oldArmor.armorType != oldEquipSlot.acceptedArmorType)
                {
                    Debug.Log("Rejected swap: armor type mismatch for old slot");
                    return;
                }
            }

            // ✅ Passed all checks, do swap
            equipmentManager.Unequip(existingItem.item);
            existingItem.transform.SetParent(oldTarget);
            existingItem.transform.localPosition = Vector3.zero;
        }

        // ✅ Drop dragged item into this slot
        draggedItem.parentAfterDrag = targetParent;
        draggedItem.transform.SetParent(targetParent);
        draggedItem.transform.localPosition = Vector3.zero;

        // ✅ Equip new item
        equipmentManager.Equip(draggedItem.item);
        Debug.Log($"Equipped: {draggedItem.item.itemName}");
    }
}
