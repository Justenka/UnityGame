using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUIGroup;
    public GameObject inventoryButton;
    public TooltipUI tooltipUI;

    void Start()
    {
        // Assign tooltip to static reference used by InventoryItem
        InventoryItem.tooltip = tooltipUI;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        bool isActive = inventoryUIGroup.activeSelf;

        inventoryUIGroup.SetActive(!isActive);
        inventoryButton.SetActive(isActive);

        if (!inventoryUIGroup.activeSelf)
            tooltipUI.Hide(); // hide if closing
    }

    public void OpenInventory()
    {
        inventoryUIGroup.SetActive(true);
        inventoryButton.SetActive(false);
    }

    public void CloseInventory()
    {
        tooltipUI.Hide(); // <-- Now this works because tooltip is assigned
        inventoryUIGroup.SetActive(false);
        inventoryButton.SetActive(true);
    }
}
