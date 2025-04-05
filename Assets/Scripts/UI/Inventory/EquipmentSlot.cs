using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public EquipmentManager equipmentManager;
    public ArmorType acceptedArmorType; // Only relevant if Armor

    public override void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (draggedItem == null)
            return;

        //  Validate by acceptedType (from InventorySlot)
        if (acceptedType != ItemType.All && draggedItem.item.type != acceptedType)
        {
            Debug.Log($"Rejected: {draggedItem.item.itemName} is type {draggedItem.item.type}, expected {acceptedType}");
            return;
        }

        //  If armor, check the specific armor type
        if (draggedItem.item is ArmorItem armorItem && acceptedType == ItemType.Armor)
        {
            if (armorItem.armorType != acceptedArmorType)
            {
                Debug.Log($"Rejected: {armorItem.itemName} is {armorItem.armorType}, expected {acceptedArmorType}");
                return;
            }
        }

        // ✅ Passed all checks → proceed with drop
        base.OnDrop(eventData);
    }
}