using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType acceptedType;

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on " + gameObject.name);

        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        // Check item type
        if (acceptedType != ItemType.All && draggedItem.item.type != acceptedType)
        {
            Debug.Log("Item type not accepted by " + gameObject.name);
            return;
        }

        // Find ItemContainer if it exists
        Transform container = transform.Find("ItemContainer");
        Transform targetParent = container != null ? container : transform;

        // Get current item in this slot
        InventoryItem existingItem = targetParent.GetComponentInChildren<InventoryItem>();

        if (existingItem != null && existingItem != draggedItem)
        {
            // Unequip the existing item if this is an equipment slot
            EquipmentSlot thisSlot = GetComponent<EquipmentSlot>();
            if (thisSlot != null && thisSlot.equipmentManager != null)
            {
                thisSlot.equipmentManager.Unequip(existingItem.item);
                Debug.Log("Unequipped during swap: " + existingItem.item.itemName);
            }

            // Swap the existing item back to dragged-from slot
            Transform oldContainer = draggedItem.parentAfterDrag.Find("ItemContainer");
            Transform oldTargetParent = oldContainer != null ? oldContainer : draggedItem.parentAfterDrag;

            existingItem.transform.SetParent(oldTargetParent);
            existingItem.transform.localPosition = Vector3.zero;
        }

        // Place dragged item into this slot
        draggedItem.parentAfterDrag = targetParent;
        draggedItem.transform.SetParent(targetParent);
        draggedItem.transform.localPosition = Vector3.zero;

        Debug.Log("Item " + draggedItem.item.itemName + " dropped into " + targetParent.name);
    }
}