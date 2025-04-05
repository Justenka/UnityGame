using UnityEngine.EventSystems;
using UnityEngine;

public class EquipmentSlot : InventorySlot
{
    public EquipmentManager equipmentManager;

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
    }
}