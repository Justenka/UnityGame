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

        if (inventoryUIGroup.activeSelf)
        {
            UIManager.Instance.RegisterOpenMenu(inventoryUIGroup);
            Debug.Log(UIManager.Instance.openMenus.Count);
        }
        else
            UIManager.Instance.UnregisterMenu(inventoryUIGroup);

        if (!inventoryUIGroup.activeSelf)
            tooltipUI.Hide();
    }

    public void OpenInventory()
    {
        
        inventoryUIGroup.SetActive(true);
        inventoryButton.SetActive(false);
    }

    public void CloseInventory()
    {
        tooltipUI.Hide();
        inventoryUIGroup.SetActive(false);
        inventoryButton.SetActive(true);
        UIManager.Instance.UnregisterMenu(inventoryUIGroup);
    }
}
