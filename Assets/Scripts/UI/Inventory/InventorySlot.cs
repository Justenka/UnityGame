using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType acceptedType;

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on " + gameObject.name);
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            if (inventoryItem != null &&
                (acceptedType == ItemType.All || inventoryItem.item.type == acceptedType))
            {
                Debug.Log("Item " + inventoryItem.item.itemName + " accepted by " + gameObject.name);
                // Set new parent
                inventoryItem.parentAfterDrag = transform;

                // Reparent the item
                inventoryItem.transform.SetParent(transform);
                inventoryItem.transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.Log("Item not accepted by " + gameObject.name);
            }
        }
        else
        {
            Debug.Log("Slot " + gameObject.name + " already has a child.");
        }
    }
}
