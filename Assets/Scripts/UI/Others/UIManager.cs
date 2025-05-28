using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiDimmer;

    public static UIManager Instance;

    public GameObject pauseMenu; 
    public GameObject settingsPanel;

    public GameObject coreInventory;
    public GameObject storageInventory;
    public TooltipUI tooltipUI;

    public List<GameObject> openMenus = new List<GameObject>();
    public bool IsAnyMenuOpen => openMenus.Count > 0;
    void Start()
    {
        InventoryItem.tooltip = tooltipUI;
    }

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }

        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            HandleEscape();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void HandleEscape()
    {
        if (openMenus.Count > 0)
        {
            CloseAllMenus();
            Time.timeScale = 1;
        }
        else
        {
            // Toggle pause menu
            if (!settingsPanel.activeSelf)
            {
                bool isOpen = pauseMenu.activeSelf;
                pauseMenu.SetActive(!isOpen);
                if (!isOpen) RegisterOpenMenu(pauseMenu);
                else UnregisterMenu(pauseMenu);
            }
            else
            {
                settingsPanel.SetActive(false);
            }
        }
    }

    public void RegisterOpenMenu(GameObject menu)
    {
        if (!openMenus.Contains(menu))
            openMenus.Add(menu);

        if (uiDimmer != null)
            uiDimmer.SetActive(true);
    }

    public void UnregisterMenu(GameObject menu)
    {
        openMenus.Remove(menu);

        if (openMenus.Count == 0 && uiDimmer != null)
            uiDimmer.SetActive(false);
    }

    public void CloseAllMenus()
    {
        foreach (var menu in openMenus)
        {
            if (menu != null)
            {
                menu.SetActive(false);
                // Special handling for UpgradeUI (return items)
                if (menu.TryGetComponent(out UpgradeUI upgradeUI))
                    upgradeUI.ReturnItemsToInventory();
            }
        }

        openMenus.Clear();

        if (coreInventory.activeSelf)
            coreInventory.SetActive(false);

        if (storageInventory.activeSelf)
            storageInventory.SetActive(false);

        if (uiDimmer != null)
            uiDimmer.SetActive(false);

        tooltipUI.Hide();
    }

    public void ToggleInventory()
    {
        if (!coreInventory.activeSelf && IsAnyMenuOpen)
            return;

        if (!coreInventory.activeSelf)
        {
            coreInventory.SetActive(true);
            storageInventory.SetActive(true);
            RegisterOpenMenu(coreInventory);
            RegisterOpenMenu(storageInventory);
        }
        else
        {
            coreInventory.SetActive(false);
            storageInventory.SetActive(false);
            UnregisterMenu(coreInventory);
            UnregisterMenu(storageInventory);
            tooltipUI.Hide();
        }
    }
}
