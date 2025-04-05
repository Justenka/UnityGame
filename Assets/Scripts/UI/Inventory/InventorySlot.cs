using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType acceptedType;

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on " + gameObject.name);

        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (inventoryItem != null &&
            (acceptedType == ItemType.All || inventoryItem.item.type == acceptedType))
        {
            // Find ItemContainer if it exists
            Transform container = transform.Find("ItemContainer");
            Transform targetParent = container != null ? container : transform;

            inventoryItem.parentAfterDrag = targetParent;

            inventoryItem.transform.SetParent(targetParent);
            inventoryItem.transform.localPosition = Vector3.zero;

            Debug.Log("Item " + inventoryItem.item.itemName + " accepted by " + targetParent.name);
        }
        else
        {
            Debug.Log("Item not accepted by " + gameObject.name);
        }
    }
}
