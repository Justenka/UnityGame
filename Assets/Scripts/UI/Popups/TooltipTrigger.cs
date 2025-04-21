using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item item;

    public void SetItem(Item i)
    {
        item = i;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && InventoryItem.tooltip != null)
        {
            InventoryItem.tooltip.Show(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryItem.tooltip != null)
        {
            InventoryItem.tooltip.Hide();
        }
    }
}
