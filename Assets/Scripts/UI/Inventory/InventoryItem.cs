using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
                              IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    private Image image;
    public Text countText;
    public Item item;
    public int count;
    [HideInInspector] public Transform parentAfterDrag;

    public static TooltipUI tooltip; // Assign this in InventoryManager

    void Start()
    {
        image = GetComponent<Image>();

        if (tooltip == null)
        {
            tooltip = Object.FindFirstObjectByType<TooltipUI>();
        }

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

        InventoryItem.tooltip.Hide();
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
    }

    // Tooltip triggers
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && tooltip != null)
        {
            tooltip.Show(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}