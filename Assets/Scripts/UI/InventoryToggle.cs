using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUIGroup;
    public GameObject inventoryButton;

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
    }

    public void OpenInventory()
    {
        inventoryUIGroup.SetActive(true);
        inventoryButton.SetActive(false);
    }

    public void CloseInventory()
    {
        inventoryUIGroup.SetActive(false);
        inventoryButton.SetActive(true);
    }
}
