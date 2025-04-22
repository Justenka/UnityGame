using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType acceptedType = ItemType.All;

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on " + gameObject.name);

        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        // ✅ 1. Validate item type
        if (acceptedType != ItemType.All && draggedItem.item.type != acceptedType)
        {
            Debug.Log($"Rejected drop: {draggedItem.item.itemName} is type {draggedItem.item.type}, expected {acceptedType}");
            return;
        }

        // ✅ 2. Identify container targets
        Transform container = transform.Find("ItemContainer");
        Transform targetParent = container != null ? container : transform;

        InventoryItem existingItem = targetParent.GetComponentInChildren<InventoryItem>();

        Transform oldContainer = draggedItem.parentAfterDrag.Find("ItemContainer");
        Transform oldTarget = oldContainer != null ? oldContainer : draggedItem.parentAfterDrag;
        InventorySlot oldSlot = oldTarget.GetComponentInParent<InventorySlot>();

        // ✅ 3. Validate and perform swap if needed
        if (existingItem != null && existingItem != draggedItem)
        {
            if (oldSlot != null && oldSlot.acceptedType != ItemType.All &&
                existingItem.item.type != oldSlot.acceptedType)
            {
                Debug.Log($"Rejected swap: {existingItem.item.itemName} can't go into {oldSlot.name}");
                return;
            }

            // ✅ EquipmentType check if swapping between equipment slots
            if (existingItem.item is EquipmentItem existingEquip && oldSlot is EquipmentSlot oldEquipSlot)
            {
                if (existingEquip.equipmentType != oldEquipSlot.acceptedEquipmentType)
                {
                    Debug.Log("Rejected swap: equipment type mismatch for old slot");
                    return;
                }
            }

            // ✅ Swap item back to original slot
            existingItem.transform.SetParent(oldTarget);
            existingItem.transform.localPosition = Vector3.zero;

            // Re-equip the swapped-back item if it's equipment
            if (oldSlot is EquipmentSlot backEquipSlot)
            {
                backEquipSlot.equipmentManager?.Equip(existingItem.item);
                Debug.Log("Re-equipped swapped-back item: " + existingItem.item.itemName);
            }
        }

        // ✅ 4. Unequip dragged item if it was equipped
        EquipmentSlot sourceEquipSlot = draggedItem.parentAfterDrag.GetComponent<EquipmentSlot>();
        sourceEquipSlot?.equipmentManager?.Unequip(draggedItem.item);

        // ✅ 5. Drop dragged item into this slot
        draggedItem.parentAfterDrag = targetParent;
        draggedItem.transform.SetParent(targetParent);
        draggedItem.transform.localPosition = Vector3.zero;

        Debug.Log($"Item {draggedItem.item.itemName} dropped into {gameObject.name}");
    }
}
