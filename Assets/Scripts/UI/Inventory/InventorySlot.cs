using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType acceptedType;

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on " + gameObject.name);

        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        // ✅ 1. Check if dragged item is valid for this slot
        if (acceptedType != ItemType.All && draggedItem.item.type != acceptedType)
        {
            Debug.Log("Rejected drop: " + draggedItem.item.itemName + " is type " + draggedItem.item.type + ", expected " + acceptedType);
            return;
        }

        // Get container for layout
        Transform container = transform.Find("ItemContainer");
        Transform targetParent = container != null ? container : transform;

        // Item currently in this slot
        InventoryItem existingItem = targetParent.GetComponentInChildren<InventoryItem>();

        // Info about the old slot of the dragged item
        Transform oldContainer = draggedItem.parentAfterDrag.Find("ItemContainer");
        Transform oldTarget = oldContainer != null ? oldContainer : draggedItem.parentAfterDrag;
        InventorySlot oldSlot = oldTarget.GetComponentInParent<InventorySlot>();

        // ✅ 2. If there's an item in this slot, validate its swap destination
        if (existingItem != null && existingItem != draggedItem)
        {
            if (oldSlot != null && oldSlot.acceptedType != ItemType.All &&
                existingItem.item.type != oldSlot.acceptedType)
            {
                Debug.Log("Rejected swap: " + existingItem.item.itemName + " is type " + existingItem.item.type + ", can't go into " + oldSlot.name);
                return;
            }

            if (existingItem.item is ArmorItem oldArmor && oldSlot is EquipmentSlot oldEquipSlot &&
                oldEquipSlot.acceptedType == ItemType.Armor &&
                oldArmor.armorType != oldEquipSlot.acceptedArmorType)
            {
                Debug.Log("Rejected swap: armor type mismatch for old slot");
                return;
            }

            // ✅ Passed checks → swap existing item back to original slot
            existingItem.transform.SetParent(oldTarget);
            existingItem.transform.localPosition = Vector3.zero;

            // Equip the swapped-in item if necessary
            EquipmentSlot oldEquip = oldTarget.GetComponent<EquipmentSlot>();
            EquipmentManager oldManager = oldEquip?.equipmentManager;

            if (oldManager != null)
            {
                oldManager.Equip(existingItem.item);
                Debug.Log("Equipped swapped-back item: " + existingItem.item.itemName);
            }
        }

        // ✅ 3. Drop dragged item into this slot
        EquipmentSlot sourceEquipSlot = draggedItem.parentAfterDrag.GetComponent<EquipmentSlot>();
        EquipmentManager sourceManager = sourceEquipSlot != null ? sourceEquipSlot.equipmentManager : null;

        // Unequip dragged item if it came from equipment and is equipped
        if (sourceManager != null && sourceManager.IsEquipped(draggedItem.item))
        {
            sourceManager.Unequip(draggedItem.item);
            Debug.Log("Unequipped dragged item: " + draggedItem.item.itemName);
        }

        draggedItem.parentAfterDrag = targetParent;
        draggedItem.transform.SetParent(targetParent);
        draggedItem.transform.localPosition = Vector3.zero;

        Debug.Log("Item " + draggedItem.item.itemName + " dropped into " + gameObject.name);
    }
}
