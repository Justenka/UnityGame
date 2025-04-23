using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public EquipmentManager equipmentManager;
    public EquipmentType acceptedEquipmentType;
    PlayerAudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null || !(draggedItem.item is EquipmentItem draggedEquip)) return;

        // Check equipment type
        if (draggedEquip.equipmentType != acceptedEquipmentType)
        {
            Debug.Log($"Rejected: {draggedEquip.itemName} is {draggedEquip.equipmentType}, expected {acceptedEquipmentType}");
            return;
        }

        // Find container
        Transform container = transform.Find("ItemContainer");
        Transform targetParent = container != null ? container : transform;

        InventoryItem existingItem = targetParent.GetComponentInChildren<InventoryItem>();

        if (existingItem != null && existingItem != draggedItem)
        {
            Transform oldContainer = draggedItem.parentAfterDrag.Find("ItemContainer");
            Transform oldTarget = oldContainer != null ? oldContainer : draggedItem.parentAfterDrag;

            InventorySlot oldSlot = oldTarget.GetComponentInParent<InventorySlot>();

            if (oldSlot != null && oldSlot is EquipmentSlot oldEquipSlot)
            {
                if (!(existingItem.item is EquipmentItem existingEquip)) return;

                if (existingEquip.equipmentType != oldEquipSlot.acceptedEquipmentType)
                {
                    Debug.Log("Rejected swap: equipment type mismatch for old slot");
                    return;
                }
            }

            equipmentManager.Unequip(existingItem.item);
            existingItem.transform.SetParent(oldTarget);
            existingItem.transform.localPosition = Vector3.zero;
        }

        draggedItem.parentAfterDrag = targetParent;
        draggedItem.transform.SetParent(targetParent);
        draggedItem.transform.localPosition = Vector3.zero;

        equipmentManager.Equip(draggedItem.item);
        audioManager.PlaySound(audioManager.gearing);
        Debug.Log($"Equipped: {draggedItem.item.itemName}");
    }
}
