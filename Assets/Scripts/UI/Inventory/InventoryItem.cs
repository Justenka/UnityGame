using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    private Image image;
    public Text countText;
    public Item item;
    public int count;
    [HideInInspector] public Transform parentAfterDrag;

    void Start()
    {
        image = GetComponent<Image>();
        if (item != null)
        {
            InitialiseItem(item);
        }
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        if (image == null)
            image = GetComponent<Image>();

        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        // Detect if dragging from an EquipmentSlot unequip
        EquipmentSlot equipmentSlot = parentAfterDrag.GetComponent<EquipmentSlot>();
        if (equipmentSlot != null && equipmentSlot.equipmentManager != null)
        {
            equipmentSlot.equipmentManager.Unequip(item);
            Debug.Log(" Unequipped: " + item.itemName);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;

        Debug.Log("OnEndDrag: parentAfterDrag is " + parentAfterDrag.name);
        EquipmentSlot equipmentSlot = parentAfterDrag.GetComponent<EquipmentSlot>();
        if (equipmentSlot != null && equipmentSlot.equipmentManager != null)
        {
            Debug.Log(" Equipping item via OnEndDrag: " + item.itemName);
            equipmentSlot.equipmentManager.Equip(item);
        }
        else
        {
            Debug.Log("OnEndDrag: Not dropped on an EquipmentSlot");
        }
    }
}
